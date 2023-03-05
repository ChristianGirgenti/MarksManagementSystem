using MarksManagementSystem.Data.Models;
using MarksManagementSystem.Data.Repositories;
using MarksManagementSystem.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace MarksManagementSystem.Pages.Teachers
{
    public class EditTeacherModel : PageModel
    {
        private ITeacherRepository _teacherRepository;
        private const int SQL_UNIQUE_CONSTRAINT_EX = 2601;
        private const int SQL_UNIQUE_CONSTRAINT_EX2 = 2627;

        [FromQuery(Name = "Id")]
        public int Id { get; set; }

        [BindProperty]
        public Teacher? EditTeacher { get; set; }

        public EditTeacherModel(ITeacherRepository teacherRepository) 
        {
            _teacherRepository = teacherRepository;
        }

        public void OnGet()
        {
            EditTeacher = _teacherRepository.GetById(Id);
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                foreach (var entry in ModelState.Values)
                {
                    foreach (var error in entry.Errors)
                    {
                        var x = error.ErrorMessage;
                    }
                }

                return Page();
            }

            try
            {
                if (EditTeacher != null)
                {
                    FormatNewTeacherValues();
                    EditTeacher.Id = Id;
                    _teacherRepository.Update(EditTeacher);
                }
                return RedirectToPage("ViewAllTeachers");
            }
            catch (Exception ex)
            {
                if (ex.InnerException is SqlException sqlEx && (sqlEx.Number == SQL_UNIQUE_CONSTRAINT_EX || sqlEx.Number == SQL_UNIQUE_CONSTRAINT_EX2))
                {
                    ModelState.AddModelError("EditTeacher.Email", "A teacher with the same email address already exists.");
                }
                return Page();
            }

        }

        public void FormatNewTeacherValues()
        {
            EditTeacher.Email = EditTeacher.Email.ToLower();
            EditTeacher.Name = StringUtilities.Capitalise(EditTeacher.Name);
            EditTeacher.LastName = StringUtilities.Capitalise(EditTeacher.LastName);
        }
    }
}
