using MarksManagementSystem.Data.Models;
using MarksManagementSystem.Data.Repositories;
using MarksManagementSystem.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography;

namespace MarksManagementSystem.Pages.Students
{
    [Authorize(Policy = "Admin")]
    public class AddStudentModel : PageModel
    {
        private readonly IStudentRepository _studentRepository;
        private const int SQL_UNIQUE_CONSTRAINT_EX = 2601;
        private const int SQL_UNIQUE_CONSTRAINT_EX2 = 2627;
        private bool EmailNeedsChange = false;
        private readonly IPasswordCreator _passwordCreator;

        [BindProperty]
        public Student NewStudent { get; set; } = new Student();

        public AddStudentModel(IStudentRepository studentRepository, IPasswordCreator passwordCreator)
        {
            _studentRepository = studentRepository ?? throw new ArgumentNullException(nameof(studentRepository));
            _passwordCreator = passwordCreator ?? throw new ArgumentNullException(nameof(passwordCreator));
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) return Page();
            if (NewStudent == null) throw new ArgumentNullException(nameof(NewStudent));
            AddStudent(NewStudent);
            return RedirectToPage("ViewAllStudents");
        }

        public void SetNewStudentValues(Student newStudent)
        {
            if (newStudent == null) throw new ArgumentNullException(nameof(newStudent));
            var nameLower = newStudent.StudentFirstName.ToLower();
            var lastNameLower = newStudent.StudentLastName.ToLower();
            newStudent.StudentFirstName = StringUtilities.Capitalise(nameLower);
            newStudent.StudentLastName = StringUtilities.Capitalise(lastNameLower);

            var password = string.Concat(newStudent.StudentFirstName.AsSpan(0, 1), lastNameLower.AsSpan(0, 1), newStudent.StudentDateOfBirth.ToString("ddMMyy"), ".");
            byte[] salt = RandomNumberGenerator.GetBytes(128 / 8); // divide by 8 to convert bits to bytes
            newStudent.PasswordSalt = salt;
            newStudent.StudentPassword = _passwordCreator.GenerateHashedPassword(salt, password);

            newStudent.StudentEmail = nameLower + "." + lastNameLower + "@myuniversity.co.uk";
            newStudent.StudentEmail = newStudent.StudentEmail.Replace(" ","");
        }

        public void AddStudent(Student newStudent)
        {
            if (newStudent == null) throw new ArgumentNullException(nameof(newStudent));

            if (!EmailNeedsChange) SetNewStudentValues(newStudent);

            try
            {
                _studentRepository.Add(newStudent);
            }
            catch (Exception ex)
            {
                //When a student with a same first name and last name of an already existing student is added,
                //the system will add a random number between 1 and 99 to the email.
                if (ex.InnerException is SqlException sqlEx && (sqlEx.Number == SQL_UNIQUE_CONSTRAINT_EX || sqlEx.Number == SQL_UNIQUE_CONSTRAINT_EX2))
                {
                    Random random = new();
                    var randomNumber = random.Next(1, 100).ToString();
                    int indexOfAt = newStudent.StudentEmail.IndexOf("@");
                    newStudent.StudentEmail = newStudent.StudentEmail.Insert(indexOfAt, randomNumber);
                    EmailNeedsChange = true;
                    AddStudent(newStudent);
                }

            }
        }
    }
}
