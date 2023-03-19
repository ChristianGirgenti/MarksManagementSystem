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
        private bool EmailNeedsChange = false;

        [BindProperty]
        public Tutor NewTutor { get; set; } = new Tutor();

        public AddTutorModel(ITutorRepository tutorRepository)
        {
            _tutorRepository = tutorRepository;
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) return Page();      
            if (NewTutor == null) throw new ArgumentNullException(nameof(NewTutor));
            AddTutor(NewTutor);
            return RedirectToPage("ViewAllTutors");         
        }

        public void SetNewTutorValues(Tutor newTutor)
        {
            if (newTutor == null) throw new ArgumentNullException(nameof(newTutor));
            newTutor.TutorEmail = newTutor.TutorEmail.ToLower();
            var nameLower = newTutor.TutorFirstName.ToLower();
            var lastNameLower = newTutor.TutorLastName.ToLower();
            newTutor.TutorFirstName = StringUtilities.Capitalise(nameLower);
            newTutor.TutorLastName = StringUtilities.Capitalise(lastNameLower);
            newTutor.TutorPassword = newTutor.TutorFirstName.Substring(0,1) + lastNameLower.Substring(0,1) + newTutor.TutorDateOfBirth.ToString("ddMMyy") + ".";
            newTutor.TutorEmail = nameLower + "." + lastNameLower + "@myUniversity.co.uk";
        }

        public void AddTutor(Tutor newTutor)
        {
            if (newTutor == null) throw new ArgumentNullException(nameof(newTutor));

            if (!EmailNeedsChange) SetNewTutorValues(newTutor);

            try
            {
                _tutorRepository.Add(newTutor);
            }
            catch (Exception ex)
            {
                //When a tutor with a same first name and last name of an already existing tutor is added,
                //the system will add a random number between 1 and 99 to the email.
                if (ex.InnerException is SqlException sqlEx && (sqlEx.Number == SQL_UNIQUE_CONSTRAINT_EX || sqlEx.Number == SQL_UNIQUE_CONSTRAINT_EX2))
                {
                    Random random = new();
                    var randomNumber = random.Next(1, 100).ToString();
                    int indexOfAt = newTutor.TutorEmail.IndexOf("@");
                    newTutor.TutorEmail = newTutor.TutorEmail.Insert(indexOfAt, randomNumber);
                    EmailNeedsChange = true;
                    AddTutor(newTutor);
                }
            }
        }
    }
}
