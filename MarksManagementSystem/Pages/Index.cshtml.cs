using MarksManagementSystem.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MarksManagementSystem.Data.Models;
using System.Numerics;
using MarksManagementSystem.Helpers;
using MarksManagementSystem.Pages.Account;

namespace MarksManagementSystem.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ICourseRepository _courseRepository;
        private readonly ICourseTutorRepository _courseTutorRepository;
        private readonly ITutorRepository _tutorRepository;
        private readonly IPasswordCreator _passwordCreator;
        private string HashedInitialPassword { get; set; } = string.Empty;
        public bool HasCoursesWithoutUnitLeader { get; set; } = new();



        public IndexModel(ICourseRepository courseRepository, ICourseTutorRepository courseTutorRepository, ITutorRepository tutorRepository, IPasswordCreator passwordCreator)
        {
            _courseRepository = courseRepository;
            _courseTutorRepository = courseTutorRepository;
            _tutorRepository = tutorRepository;
            _passwordCreator = passwordCreator;
        }

        public void OnGet()
        {
            var accoountClaims = new AccountClaims(HttpContext.User.Claims.ToList());

            var startPassword = string.Concat(accoountClaims.AccountFirstName.AsSpan(0, 1),
                                              accoountClaims.AccountLastName.AsSpan(0, 1),
                                              accoountClaims.AccountDateOfBirth,
                                              ".");

            HashedInitialPassword = _passwordCreator.GenerateHashedPassword(accoountClaims.AccountPasswordSalt, startPassword);

            ViewData["HashedInitialPassword"] = HashedInitialPassword;
            ViewData["CurrentPassword"] = accoountClaims.AccountPassword;
        }
    }
}