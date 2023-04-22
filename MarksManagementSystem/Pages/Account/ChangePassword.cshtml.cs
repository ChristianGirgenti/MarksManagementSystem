using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using MarksManagementSystem.Services.Interfaces;

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

        private readonly IChangePasswordService _changePasswordService;
    

        public ChangePasswordModel(IChangePasswordService changePasswordService)
        {
            _changePasswordService = changePasswordService;

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
                try
                {
                    await _changePasswordService.ChangePassword(this.HttpContext, CurrentPassword, NewPassword);
                    TempData["SuccessUpdate"] = "Password has been changed successfully";
                    return RedirectToPage("/Account/Login");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("NewPassword", ex.Message); 
                }
            }
            return Page();
        }

    }
}
