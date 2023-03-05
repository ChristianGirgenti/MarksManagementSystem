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
        private readonly ITeacherRepository _teacherRepository;
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
            if (!ModelState.IsValid) return Page();
            
            try
            {
                if (EditTeacher == null) throw new ArgumentNullException(nameof(EditTeacher));
                UpdateTeacher(EditTeacher);
                return RedirectToPage("ViewAllTeachers");
            }
            catch (Exception ex)
            {
                if (ex.InnerException is SqlException sqlEx && (sqlEx.Number == SQL_UNIQUE_CONSTRAINT_EX || sqlEx.Number == SQL_UNIQUE_CONSTRAINT_EX2))
                    ModelState.AddModelError("EditTeacher.Email", "A teacher with the same email address already exists.");
                
                return Page();
            }

        }

        public void FormatEditTeacherValues(Teacher editTeacher)
        {
            editTeacher.Email = editTeacher.Email.ToLower();
            var nameLower = editTeacher.Name.ToLower();
            var lastNameLower = editTeacher.LastName.ToLower();
            editTeacher.Name = StringUtilities.Capitalise(nameLower);
            editTeacher.LastName = StringUtilities.Capitalise(lastNameLower);
        }

        public void UpdateTeacher(Teacher teacher)
        {
            FormatEditTeacherValues(teacher);
            teacher.Id = Id;
            _teacherRepository.Update(teacher);
        }
    }
}
