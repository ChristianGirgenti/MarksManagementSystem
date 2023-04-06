using MarksManagementSystem.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using MarksManagementSystem.Services.Interfaces;

namespace MarksManagementSystem.Pages.Tutors
{
    [Authorize(Policy = "Admin")]
    public class AddTutorModel : PageModel
    {
        private readonly IAddTutorService _addTutorService;

        [BindProperty]
        public Tutor NewTutor { get; set; } = new Tutor();

        public AddTutorModel(IAddTutorService addTutorService)
        {
            _addTutorService = addTutorService ?? throw new ArgumentNullException(nameof(addTutorService));
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) return Page();      
            if (NewTutor == null) throw new ArgumentNullException(nameof(NewTutor));
            _addTutorService.AddTutor(NewTutor);
            return RedirectToPage("ViewAllTutors");         
        }
    }
}
