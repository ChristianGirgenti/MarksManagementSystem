using MarksManagementSystem.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MarksManagementSystem.Services.Interfaces;

namespace MarksManagementSystem.Pages.Students
{
    [Authorize(Policy = "Admin")]
    public class AddStudentModel : PageModel
    {
        private readonly IAddStudentService _addStudentService;

        [BindProperty]
        public Student NewStudent { get; set; } = new Student();

        public AddStudentModel(IAddStudentService addStudentService)
        {
            _addStudentService = addStudentService ?? throw new ArgumentNullException(nameof(addStudentService));
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) return Page();
            if (NewStudent == null) throw new ArgumentNullException(nameof(NewStudent));
            _addStudentService.AddStudent(NewStudent);
            return RedirectToPage("ViewAllStudents");
        }
    }
}
