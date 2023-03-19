using MarksManagementSystem.Data.Models;
using MarksManagementSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Runtime.CompilerServices;
using MarksManagementSystem.Helpers;
using System.Dynamic;

namespace MarksManagementSystem.Pages
{
    public class LoginModel : PageModel
    {
        private readonly MarksManagementContext _marksManagementContext;
        private readonly IPasswordCreator _passwordCreator;

        [BindProperty]
        public Credential Credential { get; set; } = new();

        [BindProperty]
        public string UserType { get; set; } = string.Empty;

        public string ErrorMessage { get; set; } = string.Empty;


        public LoginModel(MarksManagementContext context, IPasswordCreator passwordCreator)
        {
            _marksManagementContext = context;
            _passwordCreator = passwordCreator;
        }

        public void OnGet()
        {
            var err = ModelState.ErrorCount;
            
        }



        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                if (UserType == null)
                {
                    ErrorMessage = "You need to select what type of user you are to login";
                    return Page();
                }
                ErrorMessage = "Student email and password can not be empty";
                return Page();
            }

            var isLoginSuccessfull = false;
            if (UserType == "tutor")
            {
                isLoginSuccessfull = await LogInTutorIsSuccess();
            }
            else
            {
                //Log In Student
            }
            
            if (!isLoginSuccessfull)
            {
                ErrorMessage = "Invalid email or password";
                return Page();
            }
            return RedirectToPage("/Index");
        }

        private async Task<bool> LogInTutorIsSuccess()
        {

            var tutor = await _marksManagementContext.Tutor.FirstOrDefaultAsync(x => x.TutorEmail == Credential.Email);
            
            if (tutor == null) throw new ArgumentNullException(nameof(tutor));
            if (tutor.PasswordSalt == null) throw new ArgumentNullException(nameof(tutor.PasswordSalt));
            var hashedPassword = _passwordCreator.GenerateHashedPassword(tutor.PasswordSalt, Credential.Password);
            if (tutor.TutorPassword == hashedPassword)
            {
                //Login is success so create claims for the user
                var claims = new List<Claim>
                                {
                                    new Claim(ClaimTypes.Name, tutor.TutorFirstName),
                                    new Claim(ClaimTypes.Surname, tutor.TutorLastName),
                                    new Claim(ClaimTypes.Email, tutor.TutorEmail),
                                    new Claim(ClaimTypes.Role, tutor.IsAdmin ? "Admin" : "Tutor")
                                };


                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);
                return true;
            }
            return false;
        }
    }
}
