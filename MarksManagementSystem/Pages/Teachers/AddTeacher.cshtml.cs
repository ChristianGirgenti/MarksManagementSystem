using MarksManagementSystem.Data;
using MarksManagementSystem.Data.Models;
using MarksManagementSystem.Data.Repositories;
using MarksManagementSystem.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MarksManagementSystem.Pages.Teachers
{
    public class AddTeacherModel : PageModel
    {
        private readonly ITeacherRepository teacherRepository;

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
            if (NewTeacher != null)
            {
                FormatNewTeacherValues();
                teacherRepository.Add(NewTeacher);
            }
            return RedirectToPage("ViewAllTeachers");  
        }

        public void FormatNewTeacherValues()
        {
            NewTeacher.Email = NewTeacher.Email.ToLower();
            NewTeacher.Name = StringUtilities.Capitalise(NewTeacher.Name);
            NewTeacher.LastName = StringUtilities.Capitalise(NewTeacher.LastName);
        }
    }
}
