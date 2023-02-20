using MarksManagementSystem.Data.Models;
using MarksManagementSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

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
                return RedirectToPage("/Teachers/AddTeacher");
            }
            else
            {
                ErrorMessage = "Invalid ursername or password";
                return Page();
            }
        }
    }
}
