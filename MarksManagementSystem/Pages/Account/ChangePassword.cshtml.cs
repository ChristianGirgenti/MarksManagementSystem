using MarksManagementSystem.Data;
using MarksManagementSystem.Data.Repositories;
using MarksManagementSystem.Helpers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using System.Security.Claims;

namespace MarksManagementSystem.Pages.Account
{
    public class ChangePasswordModel : PageModel
    {
        [Required]
        [DataType(DataType.Password)]
        [BindProperty]
        public string CurrentPassword { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [BindProperty]
        public string RepeatedPassword { get; set; } = string.Empty;

        [Required]
        [MinLength(8)]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$",
        ErrorMessage = "Password must contain at least one lowercase letter, one uppercase letter, one number, and one special character.")]
        [DataType(DataType.Password)]
        [BindProperty]
        public string NewPassword { get; set; } = string.Empty;

        private IPasswordCreator _passwordCreator { get; set; }
        private ITutorRepository _tutorRepository { get; set; }

        public ChangePasswordModel(IPasswordCreator passwordCreator, ITutorRepository tutorRepository)
        {
            _passwordCreator = passwordCreator;
            _tutorRepository = tutorRepository;
        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid) return Page();
            if (CurrentPassword != RepeatedPassword)
            {
                ModelState.AddModelError("RepeatedPassword", "Your current password and the repeated password did not match!");
            }
            else
            {
                var claims = HttpContext.User.Claims.ToList();
                var encodedSalt = claims.FirstOrDefault(c => c.Type == "Salt")?.Value;
                if (encodedSalt == null) throw new ArgumentNullException(nameof(encodedSalt));
                var salt = Convert.FromBase64String(encodedSalt);
                var hashedCurrentPasswordForm = _passwordCreator.GenerateHashedPassword(salt, CurrentPassword);
                var tutorId = claims.FirstOrDefault(c => c.Type == "TutorId")?.Value;
                var hashedCurrentPasswordClaim = claims.FirstOrDefault(c => c.Type == "Password")?.Value;
                if (hashedCurrentPasswordForm == hashedCurrentPasswordClaim)
                {
                    if (claims.FirstOrDefault(c => c.Type == "TutorId") != null)
                    {
                        try
                        {
                            var newHashedPassword = _passwordCreator.GenerateHashedPassword(salt, NewPassword);
                            _tutorRepository.UpdatePasswordByTutorId(Convert.ToInt32(tutorId), newHashedPassword);
                            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                            TempData["SuccessUpdate"] = "Password has been changed successfully";
                            return RedirectToPage("/Account/Login");
                        }
                        catch (Exception ex)
                        {
                            TempData["ErrorMessage"] = "An error occurred while changing the password: " + ex.Message;
                        }
                    }
                    else if (claims.FirstOrDefault(c => c.Type == "StudentId") != null)
                    {

                    }
                    else
                    {
                        ModelState.AddModelError("NewPassword", "Something went wrong while trying to change password. If the error persist, contact us through the help page.");
                    }
                }
                else
                {
                    ModelState.AddModelError("NewPassword", "The current password insterted is wrong.");
                }

            }
            return Page();

        }

    }
}
