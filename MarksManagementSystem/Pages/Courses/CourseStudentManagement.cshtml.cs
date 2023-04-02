using MarksManagementSystem.Data.Models;
using MarksManagementSystem.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MarksManagementSystem.Services.Interfaces;


namespace MarksManagementSystem.Pages.Courses
{
    public class CourseStudentManagementModel : PageModel
    {
        private readonly ICourseStudentManagementService _courseStudentManagementService;

        [FromQuery(Name = "CourseId")]
        public int CourseId { get; set; }
        public Course ThisCourse { get; set; } = new Course();
        public List<SelectListItem> AllStudents { get; set; } = new List<SelectListItem>();
        public List<CourseStudent> CurrentStudentsInTheCourse { get; set; } = new();



        public CourseStudentManagementModel(ICourseStudentManagementService courseStudentManagementService)
        {
            _courseStudentManagementService = courseStudentManagementService ?? throw new ArgumentNullException(nameof(courseStudentManagementService));
        }


        public void OnGet()
        {
            ThisCourse = _courseStudentManagementService.GetCourseById(CourseId);
            ViewData["Title"] = "Manage Students For " + ThisCourse.CourseName;
            AllStudents = _courseStudentManagementService.PopulateStudentsList(CourseId);
        }

        public IActionResult OnPost()
        {
            ThisCourse = _courseStudentManagementService.GetCourseById(CourseId);
            CurrentStudentsInTheCourse = _courseStudentManagementService.GetAllCurrentStudentsInTheCourse(CourseId);
            try
            {
                _courseStudentManagementService.ChangeCourseStudentsRelationship(Request.Form["Students"].ToList(), CurrentStudentsInTheCourse, ThisCourse);
                TempData["SuccessMessage"] = "The students and course relationship have been update successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while changing course and students relationship: " + ex.Message;
            }
            AllStudents = _courseStudentManagementService.PopulateStudentsList(CourseId);
            return RedirectToPage("ViewAllCourses");
        }
    }
}
