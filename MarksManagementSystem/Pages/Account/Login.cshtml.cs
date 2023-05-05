using MarksManagementSystem.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MarksManagementSystem.Services.Interfaces;
namespace MarksManagementSystem.Pages
{
    public class LoginModel : PageModel
    {
        private readonly ILoginService _loginService;
        
        [BindProperty]
        public Credential Credential { get; set; } = new();

        [BindProperty]
        public string UserType { get; set; } = string.Empty;

        public string ErrorMessage { get; set; } = string.Empty;

        public LoginModel(ILoginService loginService)
        {
            _loginService = loginService;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(UserType))
                {
                    ErrorMessage = "You need to select what type of user you are to login";
                    return Page();
                }
                ErrorMessage = "Email and password can not be empty";
                return Page();
            }

            bool isLoginSuccessfull;
            if (UserType == "tutor")
            {
                isLoginSuccessfull = await _loginService.LogInTutorIsSuccess(Credential, HttpContext);
                ClearTempData();
            }
            else
            {
                isLoginSuccessfull = await _loginService.LogInStudentIsSuccess(Credential, HttpContext);
                ClearTempData();
            }

            if (!isLoginSuccessfull)
            {
                ErrorMessage = "Invalid email or password";
                return Page();
            }
            return RedirectToPage("/Index");
        }

       
        private void ClearTempData()
        {
            if (TempData.ContainsKey("SuccessUpdate"))
            {
                TempData.Remove("SuccessUpdate");
            }
        }
    }
}
