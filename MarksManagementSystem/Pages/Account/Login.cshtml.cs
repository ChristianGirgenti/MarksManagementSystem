using MarksManagementSystem.Data.Models;
using MarksManagementSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

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
        public string ErrorMessage { get; set; }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();
            var teacher = await _marksManagementContext.Teachers.FirstOrDefaultAsync(x => x.Email == Credential.Email);

            if (teacher != null && teacher.Password == Credential.Password)
            {
                //Create claims for the user
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, teacher.Name),
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
                return RedirectToPage("/Index");
            }
            else
            {
                ErrorMessage = "Invalid ursername or password";
                return Page();
            }
        }
    }
}
