using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using MarksManagementSystem.Data.Models;
using MarksManagementSystem.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using MarksManagementSystem.Helpers;
using MarksManagementSystem.ViewModel;

namespace MarksManagementSystem.Pages.Courses
{
    public class AddCourseModel : PageModel
    {
        private MarksManagementContext marksManagementContext;
        private const int SQL_UNIQUE_CONSTRAINT_EX = 2601;
        private const int SQL_UNIQUE_CONSTRAINT_EX2 = 2627;


        public AddCourseModel(MarksManagementContext context)
        {
            marksManagementContext = context;
        }

        [BindProperty]
        public AddCourseViewModel NewCourseViewModel { get; set; }
        public Course NewCourse { get; set; }
        public List<SelectListItem> OptionsTeachers { get; set; } = new List<SelectListItem>();
        public void OnGet()
        {
            ShowTeachersInSelectionList();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {

                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }
                return Page();
            }

            try
            {
                FormatNewCourseValues();
                NewCourse = new Course(NewCourseViewModel.Name, NewCourseViewModel.Credits);
                marksManagementContext.Courses.Add(NewCourse);
                var changes = marksManagementContext.SaveChanges();
                var headTeacher = marksManagementContext.Teachers.SingleOrDefault(t => t.Id == NewCourseViewModel.HeadTeacherId);
                if (headTeacher != null)
                {
                    var courseTeacher = new CourseTeacher { Course = NewCourse, Teacher = headTeacher, IsHeadTeacher = true };
                    marksManagementContext.CourseTeachers.Add(courseTeacher);
                }
                changes = marksManagementContext.SaveChanges();
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

            var headTeachers = marksManagementContext.CourseTeachers
                .Where(ct => ct.IsHeadTeacher)
                .Select(ct => ct.Teacher);

            var nonHeadTeachers = marksManagementContext.Teachers
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
