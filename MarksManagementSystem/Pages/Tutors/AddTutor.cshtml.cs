using MarksManagementSystem.Data.Models;
using MarksManagementSystem.Data.Repositories;
using MarksManagementSystem.Helpers;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography;
using System;
using Microsoft.AspNetCore.Authorization;

namespace MarksManagementSystem.Pages.Tutors
{
    [Authorize(Policy = "Admin")]
    public class AddTutorModel : PageModel
    {
        private readonly ITutorRepository _tutorRepository;
        private const int SQL_UNIQUE_CONSTRAINT_EX = 2601;
        private const int SQL_UNIQUE_CONSTRAINT_EX2 = 2627;
        private bool EmailNeedsChange = false;
        private readonly IPasswordCreator _passwordCreator;

        [BindProperty]
        public Tutor NewTutor { get; set; } = new Tutor();

        public AddTutorModel(ITutorRepository tutorRepository, IPasswordCreator passwordCreator)
        {
            _tutorRepository = tutorRepository ?? throw new ArgumentNullException(nameof(tutorRepository));
            _passwordCreator = passwordCreator ?? throw new ArgumentNullException(nameof(passwordCreator));
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
            
            var password = string.Concat(newTutor.TutorFirstName.AsSpan(0, 1), lastNameLower.AsSpan(0, 1), newTutor.TutorDateOfBirth.ToString("ddMMyy"), ".");
            byte[] salt = RandomNumberGenerator.GetBytes(128 / 8); // divide by 8 to convert bits to bytes
            newTutor.PasswordSalt = salt;
            newTutor.TutorPassword = _passwordCreator.GenerateHashedPassword(salt, password);
           
            newTutor.TutorEmail = nameLower + "." + lastNameLower + "@myuniversity.co.uk";
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
