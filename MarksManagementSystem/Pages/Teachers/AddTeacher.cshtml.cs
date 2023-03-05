using MarksManagementSystem.Data.Models;
using MarksManagementSystem.Data.Repositories;
using MarksManagementSystem.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace MarksManagementSystem.Pages.Teachers
{
    public class AddTeacherModel : PageModel
    {
        private readonly ITeacherRepository _teacherRepository;
        private const int SQL_UNIQUE_CONSTRAINT_EX = 2601;
        private const int SQL_UNIQUE_CONSTRAINT_EX2 = 2627;

        public AddTeacherModel(ITeacherRepository teacherRepository)
        {
            _teacherRepository = teacherRepository;
        }

        [BindProperty]
        public Teacher? NewTeacher { get; set; }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) return Page();
            try
            {
                if (NewTeacher == null) throw new ArgumentNullException(nameof(NewTeacher));
                AddTeacher(NewTeacher);
                return RedirectToPage("ViewAllTeachers");   
            }
            catch (Exception ex)
            {
                if (ex.InnerException is SqlException sqlEx && (sqlEx.Number == SQL_UNIQUE_CONSTRAINT_EX || sqlEx.Number == SQL_UNIQUE_CONSTRAINT_EX2))
                    ModelState.AddModelError("NewTeacher.Email", "A teacher with the same email address already exists.");
                return Page();
            }
            
        }

        public void FormatNewTeacherValues(Teacher newTeacher)
        {
            if (newTeacher == null) throw new ArgumentNullException(nameof(newTeacher));
            newTeacher.Email = newTeacher.Email.ToLower();
            var nameLower = newTeacher.Name.ToLower();
            var lastNameLower = newTeacher.LastName.ToLower();
            newTeacher.Name = StringUtilities.Capitalise(nameLower);
            newTeacher.LastName = StringUtilities.Capitalise(lastNameLower);
        }

        public void AddTeacher(Teacher newTeacher)
        {
            if (newTeacher == null) throw new ArgumentNullException(nameof(newTeacher));

            FormatNewTeacherValues(newTeacher);
            _teacherRepository.Add(newTeacher);          
        }
    }
}
