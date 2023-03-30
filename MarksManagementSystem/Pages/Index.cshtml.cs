using MarksManagementSystem.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MarksManagementSystem.Data.Models;
using System.Numerics;
using MarksManagementSystem.Helpers;
using MarksManagementSystem.Pages.Account;
using MarksManagementSystem.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace MarksManagementSystem.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ICourseRepository _courseRepository;
        private readonly ICourseTutorRepository _courseTutorRepository;
        private readonly ICourseStudentRepository _courseStudentRepository;
        private readonly ITutorRepository _tutorRepository;
        private readonly IPasswordCreator _passwordCreator;
        private string HashedInitialPassword { get; set; } = string.Empty;
        public AccountClaims AccountClaims { get; set; } 
        public List<ViewAllCoursesViewModel> TutorTaughtCourses { get; set; }
        public List<CourseStudent> StudentEnrolledCourses { get; set; }


        public List<CourseStudent> CourseStudents { get; set; }

        public IndexModel(
            ICourseRepository courseRepository, 
            ICourseTutorRepository courseTutorRepository, 
            ITutorRepository tutorRepository, 
            IPasswordCreator passwordCreator, 
            ICourseStudentRepository courseStudentRepository
            )
        {
            _courseRepository = courseRepository;
            _courseTutorRepository = courseTutorRepository;
            _tutorRepository = tutorRepository;
            _passwordCreator = passwordCreator;
            _courseStudentRepository = courseStudentRepository;
        }

        public void OnGet()
        {
            AccountClaims = new AccountClaims(HttpContext.User.Claims.ToList());
            var lastNameLower = AccountClaims.AccountLastName.ToLower();
            var startPassword = string.Concat(AccountClaims.AccountFirstName.AsSpan(0, 1),
                                              lastNameLower.AsSpan(0, 1),
                                              AccountClaims.AccountDateOfBirth,
                                              ".");

            HashedInitialPassword = _passwordCreator.GenerateHashedPassword(AccountClaims.AccountPasswordSalt, startPassword);

            ViewData["HashedInitialPassword"] = HashedInitialPassword;
            ViewData["CurrentPassword"] = AccountClaims.AccountPassword;

            if (AccountClaims.AccountUserType.Equals("Tutor"))
            {
                GetTutorCourses();
            }
            else
            {
                GetStudentCourses();
            }
        }

        public void GetTutorCourses()
        {
            TutorTaughtCourses = _courseTutorRepository.GetAllByTutorId(Convert.ToInt32(AccountClaims.AccountId))
               .Select(c => new ViewAllCoursesViewModel
               {
                   CourseId = c.CourseId,
                   CourseName = c.Course.CourseName,
                   CourseCredits = c.Course.CourseCredits,

                   UnitLeader = _courseTutorRepository.GetAll()
                        .Where(ct => ct.CourseId == c.CourseId && ct.IsUnitLeader == true)
                        .Select(ct => ct.Tutor.ToString())
                        .SingleOrDefault(),

                   OtherTutors = string.Join(", ", _courseTutorRepository.GetAll()
                       .Where(ct => ct.CourseId == c.CourseId && ct.IsUnitLeader == false)
                       .Select(ct => ct.Tutor.ToString())
                       .ToList())
               }) 
               .ToList();
        }

        public void GetStudentCourses()
        {
            //TO COMPLETE
            StudentEnrolledCourses = new List<CourseStudent>();
        }


        public IActionResult OnPostEdit(int courseId)
        {
            if (courseId <= 0) throw new ArgumentNullException(nameof(courseId));
            return RedirectToPage("Courses/ViewCourse", new { courseId });
        }
    }
}