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
        private readonly ICourseTutorRepository _courseTutorRepository;

        public bool HasCoursesWithoutUnitLeader { get; set; } 
        public IQueryable<Course> CoursesWithoutUnitLeader { get;set; }
        public IQueryable<Course> CourseWithoutTutors { get; set; }

        public IndexModel(ICourseRepository courseRepository, ICourseTutorRepository courseTutorRepository)
        {
            _courseRepository = courseRepository;
            _courseTutorRepository = courseTutorRepository; 
        }

        public void OnGet()
        {

        }
    }
}