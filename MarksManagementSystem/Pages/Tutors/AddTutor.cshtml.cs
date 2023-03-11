using MarksManagementSystem.Data.Models;
using MarksManagementSystem.Data.Repositories;
using MarksManagementSystem.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace MarksManagementSystem.Pages.Tutors
{
    public class AddTutorModel : PageModel
    {
        private readonly ITutorRepository _tutorRepository;
        private const int SQL_UNIQUE_CONSTRAINT_EX = 2601;
        private const int SQL_UNIQUE_CONSTRAINT_EX2 = 2627;

        public AddTutorModel(ITutorRepository tutorRepository)
        {
            _tutorRepository = tutorRepository;
        }

        [BindProperty]
        public Tutor? NewTutor { get; set; }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) return Page();
            try
            {
                if (NewTutor == null) throw new ArgumentNullException(nameof(NewTutor));
                AddTutor(NewTutor);
                return RedirectToPage("ViewAllTutors");   
            }
            catch (Exception ex)
            {
                if (ex.InnerException is SqlException sqlEx && (sqlEx.Number == SQL_UNIQUE_CONSTRAINT_EX || sqlEx.Number == SQL_UNIQUE_CONSTRAINT_EX2))
                    ModelState.AddModelError("NewTutor.Email", "A tutor with the same email address already exists.");
                return Page();
            }
            
        }

        public void FormatNewTutorValues(Tutor newTutor)
        {
            if (newTutor == null) throw new ArgumentNullException(nameof(newTutor));
            newTutor.Email = newTutor.Email.ToLower();
            var nameLower = newTutor.Name.ToLower();
            var lastNameLower = newTutor.LastName.ToLower();
            newTutor.Name = StringUtilities.Capitalise(nameLower);
            newTutor.LastName = StringUtilities.Capitalise(lastNameLower);
        }

        public void AddTutor(Tutor newTutor)
        {
            if (newTutor == null) throw new ArgumentNullException(nameof(newTutor));

            FormatNewTutorValues(newTutor);
            _tutorRepository.Add(newTutor);          
        }
    }
}
