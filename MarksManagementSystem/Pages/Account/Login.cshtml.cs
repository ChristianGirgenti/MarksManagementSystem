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

namespace MarksManagementSystem.Pages
{
    public class LoginModel : PageModel
    {
        private MarksManagementContext _marksManagementContext;

        public LoginModel(MarksManagementContext context)
        {
            _marksManagementContext = context;
        }

        [BindProperty]
        public Credential Credential { get; set; }

        [BindProperty]
        public string UserType { get; set; }
        public string ErrorMessage { get; set; }
        public void OnGet()
        {
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
                ErrorMessage = "Email and password can not be empty";
                return Page();
            }

            var isLoginSuccessfull = false;
            if (UserType == "teacher")
            {
                isLoginSuccessfull = await LogInTeacherIsSuccess();
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

        private async Task<bool> LogInTeacherIsSuccess()
        {
            var teacher = await _marksManagementContext.Teachers.FirstOrDefaultAsync(x => x.Email == Credential.Email);
            if (teacher != null && teacher.Password == Credential.Password)
            {
                //Login is success so create claims for the user
                var claims = new List<Claim>
                                {
                                    new Claim(ClaimTypes.Name, teacher.Name),
                                    new Claim(ClaimTypes.Surname, teacher.LastName),
                                    new Claim(ClaimTypes.Email, teacher.Email),
                                    new Claim(ClaimTypes.Role, teacher.IsAdmin ? "Admin" : "Teacher")
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
