using MarksManagementSystem.Data.Models;
using MarksManagementSystem.Data.Repositories;
using MarksManagementSystem.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MarksManagementSystem.Pages.Courses
{
    [Authorize(Policy = "Tutor")]

    public class ViewCourseModel : PageModel
    {
        private readonly ICourseRepository _courseRepository;
        private readonly ICourseTutorRepository _courseTutorRepository;
        private readonly ICourseStudentRepository _courseStudentRepository;


        [FromQuery(Name = "CourseId")]
        public int CourseId { get; set; }
        public Course Course { get; set; } = new Course();
        public Tutor CourseUnitLeader { get; set; } = new Tutor();
        public List<ViewCourseViewModel> AllStudentsEnrolled { get; set; } = new List<ViewCourseViewModel>();
        public AccountClaims AccountClaims;
        public bool IsUserLoggedInTheUnitLeader = false;
       


        public ViewCourseModel(ICourseRepository courseRepository,ICourseTutorRepository courseTutorRepository, ICourseStudentRepository courseStudentRepository)
        {
            _courseRepository = courseRepository;
            _courseStudentRepository = courseStudentRepository;
            _courseTutorRepository = courseTutorRepository;
        }


        public void OnGet()
        {
            Course = _courseRepository.GetById(CourseId);
            ViewData["Title"] = Course.CourseName;

            AccountClaims = new AccountClaims(HttpContext.User.Claims.ToList());
            CourseUnitLeader = _courseTutorRepository.GetUnitLeaderOfCourse(CourseId);
            if (Convert.ToInt32(AccountClaims.AccountId) == CourseUnitLeader.TutorId)
            {
                IsUserLoggedInTheUnitLeader = true;
            }

            AllStudentsEnrolled = _courseStudentRepository.GetAllByCourseId(CourseId)
               .Select(s => new ViewCourseViewModel
               {
                   StudentFullName = s.Student.StudentFirstName + " " + s.Student.StudentLastName,
                   StudentEmail = s.Student.StudentEmail,
                   StudentDateOfBirth = s.Student.StudentDateOfBirth.Date.ToString("d"),
                   StudentMark = s.Mark.ToString()
               })
               .ToList();
        }
    }
}
