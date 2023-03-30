using MarksManagementSystem.Data.Models;
using MarksManagementSystem.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MarksManagementSystem.Pages.Courses
{
    public class ViewCourseModel : PageModel
    {
        private readonly ICourseRepository _courseRepository;

        [FromQuery(Name = "CourseId")]
        public int CourseId { get; set; }
        public Course Course { get; set; } = new Course();



        public ViewCourseModel(ICourseRepository courseRepository)
        {
            _courseRepository = courseRepository;
        }


        public void OnGet()
        {
            Course = _courseRepository.GetById(CourseId);
            ViewData["Title"] = Course.CourseName;
        }
    }
}
