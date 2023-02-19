using MarksManagementSystem.Data;
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
                var studentName = NewStudent.Name;
                var studentLastName = NewStudent.LastName;
                var studentEmail = NewStudent.Email;
                var studentPassoord = NewStudent.Password;

                return RedirectToPage("ViewAllStudents");
            }
            return Page();
        }
    }
}
