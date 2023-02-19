using MarksManagementSystem.Data;
using MarksManagementSystem.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MarksManagementSystem.Pages.Teachers
{
    public class AddTeacherModel : PageModel
    {
        private MarksManagementContext marksManagementContext;

        public AddTeacherModel(MarksManagementContext context) 
        {
            marksManagementContext = context;
        }

        [BindProperty]
        public Teacher NewTeacher { get; set; }
        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) return Page();
            marksManagementContext.Teachers.Add(NewTeacher);
            var changes = marksManagementContext.SaveChanges();
            return RedirectToPage("ViewAllTeachers");  
        }
    }
}
