using MarksManagementSystem.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MarksManagementSystem.Data.Models;


namespace MarksManagementSystem.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ICourseRepository _courseRepository;
        private readonly ICourseTeacherRepository _courseTeacherRepository;

        public bool HasCoursesWithoutHeadTeacher { get; set; } 
        public IQueryable<Course> CoursesWithoutHeadTeacher { get;set; }
        public IQueryable<Course> CourseWithoutTeachers { get; set; }

        public IndexModel(ICourseRepository courseRepository, ICourseTeacherRepository courseTeacherRepository)
        {
            _courseRepository = courseRepository;
            _courseTeacherRepository = courseTeacherRepository; 
        }

        public void OnGet()
        {

        }
    }
}