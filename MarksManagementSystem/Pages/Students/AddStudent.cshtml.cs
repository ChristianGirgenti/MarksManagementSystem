using MarksManagementSystem.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MarksManagementSystem.Pages.Students
{
    public class AddStudentModel : PageModel
    {
        [BindProperty]
        public Student NewStudent { get; set; }
        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            // SAVE PRODUCT TO DATABASE
            if (ModelState.IsValid)
            {
                var studentName = NewStudent.StudentFirstName;
                var studentLastName = NewStudent.StudentLastName;
                var studentEmail = NewStudent.StudentEmail;
                var studentPassoord = NewStudent.StudentPassword;

                return RedirectToPage("ViewAllStudents");
            }
            return Page();
        }
    }
}
