using MarksManagementSystem.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MarksManagementSystem.Data.Models;
using System.Numerics;
using MarksManagementSystem.Helpers;

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
        private string CurrentPassword { get; set; } = string.Empty;
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
            var claims = HttpContext.User.Claims;
            var userFirstName = claims.FirstOrDefault(c => c.Type == "FirstName")?.Value;
            var userLastName = claims.FirstOrDefault(c => c.Type == "LastName")?.Value.ToLower();
            var password = claims.FirstOrDefault(c => c.Type == "Password")?.Value.ToString();
            var dob = claims.FirstOrDefault(c => c.Type == "DateOfBirth")?.Value.ToString();
            var encodedSalt = claims.FirstOrDefault(c => c.Type == "Salt")?.Value.ToString();
            if (encodedSalt == null) throw new ArgumentNullException(nameof(encodedSalt));
            var salt = Convert.FromBase64String(encodedSalt);


            var startPassword = string.Concat(userFirstName.AsSpan(0, 1),
                                              userLastName.AsSpan(0, 1),
                                              dob,
                                              ".");

            HashedInitialPassword = _passwordCreator.GenerateHashedPassword(salt, startPassword);
            CurrentPassword = password ?? throw new ArgumentNullException(nameof(password));


            ViewData["HashedInitialPassword"] = HashedInitialPassword;
            ViewData["CurrentPassword"] = CurrentPassword;
        }
    }
}