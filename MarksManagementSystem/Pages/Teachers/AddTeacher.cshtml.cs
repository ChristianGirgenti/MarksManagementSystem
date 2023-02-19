using MarksManagementSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MarksManagementSystem.Pages.Teachers
{
    public class AddTeacherModel : PageModel
    {
        [BindProperty]
        public Teacher NewTeacher { get; set; }
        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            // SAVE PRODUCT TO DATABASE
            if (ModelState.IsValid)
            {
                var teacherName = NewTeacher.Name;
                var teacherLastName = NewTeacher.LastName;
                var teacherEmail = NewTeacher.Email;
                var teacherPassoord = NewTeacher.Password;

                return RedirectToPage("ViewAllTeachers");
            }
            return Page();
        }
    }
}
