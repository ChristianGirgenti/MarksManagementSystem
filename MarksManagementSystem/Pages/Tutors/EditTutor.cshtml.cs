using MarksManagementSystem.Data.Models;
using MarksManagementSystem.Data.Repositories;
using MarksManagementSystem.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace MarksManagementSystem.Pages.Tutors
{
    public class EditTutorModel : PageModel
    {
        private readonly ITutorRepository _tutorRepository;
        private const int SQL_UNIQUE_CONSTRAINT_EX = 2601;
        private const int SQL_UNIQUE_CONSTRAINT_EX2 = 2627;

        [FromQuery(Name = "Id")]
        public int Id { get; set; }

        [BindProperty]
        public Tutor? EditTutor { get; set; }

        public EditTutorModel(ITutorRepository tutorRepository) 
        {
            _tutorRepository = tutorRepository;
        }

        public void OnGet()
        {
            EditTutor = _tutorRepository.GetById(Id);
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) return Page();
            
            try
            {
                if (EditTutor == null) throw new ArgumentNullException(nameof(EditTutor));
                UpdateTutor(EditTutor);
                return RedirectToPage("ViewAllTutors");
            }
            catch (Exception ex)
            {
                if (ex.InnerException is SqlException sqlEx && (sqlEx.Number == SQL_UNIQUE_CONSTRAINT_EX || sqlEx.Number == SQL_UNIQUE_CONSTRAINT_EX2))
                    ModelState.AddModelError("EditTutor.Email", "A tutor with the same email address already exists.");
                
                return Page();
            }

        }

        public void FormatEditTutorValues(Tutor editTutor)
        {
            editTutor.Email = editTutor.Email.ToLower();
            var nameLower = editTutor.Name.ToLower();
            var lastNameLower = editTutor.LastName.ToLower();
            editTutor.Name = StringUtilities.Capitalise(nameLower);
            editTutor.LastName = StringUtilities.Capitalise(lastNameLower);
        }

        public void UpdateTutor(Tutor tutor)
        {
            FormatEditTutorValues(tutor);
            tutor.Id = Id;
            _tutorRepository.Update(tutor);
        }
    }
}
