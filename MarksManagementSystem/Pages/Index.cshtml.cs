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
        private readonly ICourseTutorRepository _courseTutorRepository;
        private readonly ICourseStudentRepository _courseStudentRepository;
        private readonly IPasswordCreator _passwordCreator;
        private string HashedInitialPassword { get; set; } = string.Empty;
        public AccountClaims AccountClaims { get; set; }
        public List<ViewAllCoursesViewModel> TutorTaughtCourses { get; set; } = new List<ViewAllCoursesViewModel>();
        public List<StudentIndexViewModel> StudentIndexViewModels { get; set; } = new List<StudentIndexViewModel>();


        public IndexModel(
            ICourseTutorRepository courseTutorRepository, 
            IPasswordCreator passwordCreator, 
            ICourseStudentRepository courseStudentRepository
            )
        {
            _courseTutorRepository = courseTutorRepository;
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
            StudentIndexViewModels = _courseStudentRepository.GetAllByStudentId(Convert.ToInt32(AccountClaims.AccountId))
                .Select(c => new StudentIndexViewModel
                {
                    CourseName = c.Course.CourseName,
                    CourseCredits = c.Course.CourseCredits.ToString(),
                    Mark = c.Mark.ToString(),
                    //The ShowMark field checks that every student in that course have a mark assigned. If any of them still hasn't got a mark
                    //Keep the marks not visible to all the students.
                    ShowMark = !(_courseStudentRepository.GetAllByCourseId(c.Course.CourseId).Any(m => m.Mark == -1))
                })
                .ToList();     
        }


        public IActionResult OnPostEdit(int courseId)
        {
            if (courseId <= 0) throw new ArgumentNullException(nameof(courseId));
            return RedirectToPage("Courses/ViewCourse", new { courseId });
        }
    }
}