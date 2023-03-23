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
using Microsoft.AspNetCore.Authorization;

namespace MarksManagementSystem.Pages.Account
{
    [Authorize]

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

        private readonly IPasswordCreator _passwordCreator;

        private readonly ITutorRepository _tutorRepository;
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
                var accountClaims = new AccountClaims(HttpContext.User.Claims.ToList());

                var hashedCurrentPasswordForm = _passwordCreator.GenerateHashedPassword(accountClaims.AccountPasswordSalt, CurrentPassword);
                if (hashedCurrentPasswordForm == accountClaims.AccountPassword)
                {
                    if (accountClaims.AccountIsTutor)
                    {
                        try
                        {
                            var newHashedPassword = _passwordCreator.GenerateHashedPassword(accountClaims.AccountPasswordSalt, NewPassword);
                            _tutorRepository.UpdatePasswordByTutorId(Convert.ToInt32(accountClaims.AccountId), newHashedPassword);
                            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                            TempData["SuccessUpdate"] = "Password has been changed successfully";
                            return RedirectToPage("/Account/Login");
                        }
                        catch (Exception ex)
                        {
                            TempData["ErrorMessage"] = "An error occurred while changing the password: " + ex.Message;
                        }
                    }
                    else 
                    {

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
