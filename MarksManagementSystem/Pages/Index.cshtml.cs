using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MarksManagementSystem.Data.Models;
using MarksManagementSystem.ViewModel;
using Microsoft.AspNetCore.Mvc;
using MarksManagementSystem.Services.Interfaces;
using System.Linq.Expressions;

namespace MarksManagementSystem.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IIndexService _indexService;
        private string HashedInitialPassword { get; set; } = string.Empty;
        public AccountClaims AccountClaims { get; set; }
        public List<ViewAllCoursesViewModel> TutorTaughtCourses { get; set; } = new List<ViewAllCoursesViewModel>();
        public List<StudentIndexViewModel> StudentIndexViewModels { get; set; } = new List<StudentIndexViewModel>();


        public IndexModel(IIndexService indexService)
        {
            _indexService = indexService ?? throw new ArgumentNullException(nameof(indexService));
        }

        public void OnGet()
        {
            AccountClaims = new AccountClaims(HttpContext.User.Claims.ToList());
            HashedInitialPassword = _indexService.ConstructDefaultInitialPassword(AccountClaims);

            ViewData["HashedInitialPassword"] = HashedInitialPassword;
            ViewData["CurrentPassword"] = AccountClaims.AccountPassword;

            if (AccountClaims.AccountUserType.Equals("Tutor"))
            {
                TutorTaughtCourses = _indexService.GetTutorCourses(AccountClaims);
            }
            else
            {
                StudentIndexViewModels = _indexService.GetStudentCourses(AccountClaims);
            }
        }

        public IActionResult OnPostEdit(int courseId)
        {
            if (courseId <= 0) throw new ArgumentNullException(nameof(courseId));
            return RedirectToPage("Courses/ViewCourse", new { courseId });
        }
    }
}