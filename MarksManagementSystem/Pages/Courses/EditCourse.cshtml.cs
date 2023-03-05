using MarksManagementSystem.Data.Models;
using MarksManagementSystem.Data.Repositories;
using MarksManagementSystem.Helpers;
using MarksManagementSystem.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;

namespace MarksManagementSystem.Pages.Courses
{
    public class EditCourseModel : PageModel
    {
        private readonly ICourseRepository courseRepository;
        private readonly ITeacherRepository teacherRepository;
        private readonly ICourseTeacherRepository courseTeacherRepository;
        private const int SQL_UNIQUE_CONSTRAINT_EX = 2601;
        private const int SQL_UNIQUE_CONSTRAINT_EX2 = 2627;

        [BindProperty]
        public AddCourseViewModel? EditCourseViewModel { get; set; }
        public List<SelectListItem> OptionsTeachers { get; set; } = new List<SelectListItem>();
        
        [FromQuery(Name = "Id")]
        public int Id { get; set; }

        public EditCourseModel(ICourseRepository courseRepository, ITeacherRepository teacherRepository, ICourseTeacherRepository courseTeacherRepository)
        {
            this.courseRepository = courseRepository;
            this.teacherRepository = teacherRepository;
            this.courseTeacherRepository = courseTeacherRepository;
        }

       
        public void OnGet()
        {
            var courseToEdit = courseRepository.GetById(Id);
            courseToEdit.CourseTeachers = courseTeacherRepository.GetAll().Where(ct => ct.CourseId == Id).ToList();
            EditCourseViewModel = new AddCourseViewModel();
            EditCourseViewModel.Name = courseToEdit.Name;
            EditCourseViewModel.Credits = courseToEdit.Credits;
            EditCourseViewModel.HeadTeacherId = courseToEdit.CourseTeachers.Where(ct => ct.CourseId == Id && ct.IsHeadTeacher == true).FirstOrDefault().TeacherId;
            ShowTeachersInSelectionList(EditCourseViewModel.HeadTeacherId);
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) return Page();

            try
            {
                FormatNewCourseValues();
                var courseEdited = new Course(EditCourseViewModel.Name, EditCourseViewModel.Credits);
                courseEdited.Id = Id;
                courseRepository.Update(courseEdited);
                var newHeadTeacher = teacherRepository.GetAll().SingleOrDefault(t => t.Id == EditCourseViewModel.HeadTeacherId);
                if (newHeadTeacher != null)
                {
                    var currentCourseTeacher = courseTeacherRepository.GetAll()
                        .Where(ct => ct.CourseId == courseEdited.Id && ct.IsHeadTeacher == true)
                        .FirstOrDefault();
                    if (currentCourseTeacher != null)
                    {
                        currentCourseTeacher.Course = courseEdited;
                        currentCourseTeacher.Teacher = newHeadTeacher;
                        courseTeacherRepository.Update(currentCourseTeacher);
                    }
                }
                return RedirectToPage("ViewAllCourses");
            }
            catch (Exception ex)
            {
                if (ex.InnerException is SqlException sqlEx && (sqlEx.Number == SQL_UNIQUE_CONSTRAINT_EX || sqlEx.Number == SQL_UNIQUE_CONSTRAINT_EX2))
                {
                    ModelState.AddModelError("NewCourse.Name", "A course with the same name already exists.");
                    ShowTeachersInSelectionList(EditCourseViewModel.HeadTeacherId);
                }
                return Page();
            }
        }

        public void ShowTeachersInSelectionList(int headTeacherId)
        {

            var headTeachers = courseTeacherRepository.GetAll()
                .Where(ct => ct.IsHeadTeacher)
                .Select(ct => ct.Teacher);

            var nonHeadTeachers = teacherRepository.GetAll()
                .Where(t => !headTeachers.Contains(t))
                .Select(t => new SelectListItem { Value = t.Id.ToString(), Text = t.Name + " " + t.LastName })
                .ToList();

            OptionsTeachers = nonHeadTeachers;
            var currentHeadTeacher = headTeachers.Where(ht => ht.Id == headTeacherId).FirstOrDefault();
            OptionsTeachers.Insert(0, new SelectListItem { Value = currentHeadTeacher.Id.ToString(), Text = currentHeadTeacher.Name + " " + currentHeadTeacher.LastName });
        }

        public void FormatNewCourseValues()
        {
            EditCourseViewModel.Name = StringUtilities.Capitalise(EditCourseViewModel.Name);
        }
    }
}
