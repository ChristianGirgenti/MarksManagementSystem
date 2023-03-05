using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using MarksManagementSystem.Data.Models;
using MarksManagementSystem.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using MarksManagementSystem.Helpers;
using MarksManagementSystem.ViewModel;
using MarksManagementSystem.Data.Repositories;

namespace MarksManagementSystem.Pages.Courses
{
    public class AddCourseModel : PageModel
    {
        private readonly ICourseRepository courseRepository;
        private readonly ITeacherRepository teacherRepository;
        private readonly ICourseTeacherRepository courseTeacherRepository;
        private const int SQL_UNIQUE_CONSTRAINT_EX = 2601;
        private const int SQL_UNIQUE_CONSTRAINT_EX2 = 2627;


        public AddCourseModel(ICourseRepository courseRepository, ITeacherRepository teacherRepository, ICourseTeacherRepository courseTeacherRepository)
        {
            this.courseRepository = courseRepository;
            this.teacherRepository = teacherRepository;
            this.courseTeacherRepository = courseTeacherRepository; 
        }

        [BindProperty]
        public AddCourseViewModel? NewCourseViewModel { get; set; }
        public Course? NewCourse { get; set; }
        public List<SelectListItem> OptionsTeachers { get; set; } = new List<SelectListItem>();
        public void OnGet()
        {
            ShowTeachersInSelectionList();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) return Page();
            
            try
            {
                FormatNewCourseValues();
                NewCourse = new Course(NewCourseViewModel.Name, NewCourseViewModel.Credits);
                courseRepository.Add(NewCourse);
                var headTeacher = teacherRepository.GetAll().SingleOrDefault(t => t.Id == NewCourseViewModel.HeadTeacherId);
                if (headTeacher != null)
                {
                    var courseTeacher = new CourseTeacher { Course = NewCourse, Teacher = headTeacher, IsHeadTeacher = true };
                    courseTeacherRepository.Add(courseTeacher);
                }
                return RedirectToPage("ViewAllCourses");
            }
            catch (Exception ex) {
                if (ex.InnerException is SqlException sqlEx && (sqlEx.Number == SQL_UNIQUE_CONSTRAINT_EX || sqlEx.Number == SQL_UNIQUE_CONSTRAINT_EX2))
                {
                    ModelState.AddModelError("NewCourse.Name", "A course with the same name already exists.");
                    ShowTeachersInSelectionList();
                }
                return Page();
            }  
        }

        public void ShowTeachersInSelectionList()
        {

            var headTeachers = courseTeacherRepository.GetAll()
                .Where(ct => ct.IsHeadTeacher)
                .Select(ct => ct.Teacher);

            var nonHeadTeachers = teacherRepository.GetAll()
                .Where(t => !headTeachers.Contains(t))
                .Select(t => new SelectListItem { Value = t.Id.ToString(), Text = t.Name + " " + t.LastName })
                .ToList();

            OptionsTeachers = nonHeadTeachers;
            OptionsTeachers.Insert(0, new SelectListItem { Value = "", Text = "Select one head teacher for this course..." });
        }

        public void FormatNewCourseValues()
        {
            NewCourseViewModel.Name = StringUtilities.Capitalise(NewCourseViewModel.Name);
        }
    }
}
