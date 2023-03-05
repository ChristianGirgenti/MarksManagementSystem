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
        private readonly ITeacherRepository teacherRepository;
        private const int SQL_UNIQUE_CONSTRAINT_EX = 2601;
        private const int SQL_UNIQUE_CONSTRAINT_EX2 = 2627;

        public AddTeacherModel(ITeacherRepository teacherRepository)
        {
            this.teacherRepository = teacherRepository;
        }

        [BindProperty]
        public Teacher? NewTeacher { get; set; }
        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) return Page();
            try
            {
                if (NewTeacher != null)
                {
                    FormatNewTeacherValues();
                    teacherRepository.Add(NewTeacher);
                }
                return RedirectToPage("ViewAllTeachers");
            }
            catch (Exception ex)
            {
                if (ex.InnerException is SqlException sqlEx && (sqlEx.Number == SQL_UNIQUE_CONSTRAINT_EX || sqlEx.Number == SQL_UNIQUE_CONSTRAINT_EX2))
                {
                    ModelState.AddModelError("NewTeacher.Email", "A teacher with the same email address already exists.");
                }
                return Page();
            }
            
        }

        public void FormatNewTeacherValues()
        {
            NewTeacher.Email = NewTeacher.Email.ToLower();
            NewTeacher.Name = StringUtilities.Capitalise(NewTeacher.Name);
            NewTeacher.LastName = StringUtilities.Capitalise(NewTeacher.LastName);
        }
    }
}
