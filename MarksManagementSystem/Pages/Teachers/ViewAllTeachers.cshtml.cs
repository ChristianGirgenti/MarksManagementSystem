using MarksManagementSystem.Data;
using MarksManagementSystem.Data.Repositories;
using MarksManagementSystem.Migrations;
using MarksManagementSystem.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using System.Security.Claims;

namespace MarksManagementSystem.Pages.Teachers
{
    public class ViewAllTeachersModel : PageModel
    {
        private readonly ITeacherRepository teacherRepository;
        private readonly ICourseTeacherRepository courseTeacherRepository;
        public List<ViewAllTeachersViewModel>? AllTeachersViewModel { get; set; }

        public ViewAllTeachersModel(ITeacherRepository teacherRepository, ICourseTeacherRepository courseTeacherRepository)
        {
            this.teacherRepository = teacherRepository;
            this.courseTeacherRepository = courseTeacherRepository;
        }
        public void OnGet()
        {
            AllTeachersViewModel = teacherRepository.GetAll()
               .Select(t => new ViewAllTeachersViewModel
               {
                   TeacherId = t.Id,
                   TeacherFullName = t.Name + " " + t.LastName,
                   TeacherEmail = t.Email,
                   CourseLed = courseTeacherRepository.GetAll()
                       .Where(ct => ct.TeacherId == t.Id && ct.IsHeadTeacher == true)
                       .Select(ct => ct.Course.Name)
                       .SingleOrDefault(),

                   OtherCourses = string.Join(", ", courseTeacherRepository.GetAll()
                       .Where(ct => ct.TeacherId == t.Id && ct.IsHeadTeacher == false)
                       .Select(ct => ct.Course.Name)
                       .ToList())
               })
               .ToList();
        }

        public IActionResult OnPostDelete(int id)
        {
            try
            {
                if (teacherRepository.GetById(id).Email != (HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value))
                {
                    teacherRepository.Delete(id);
                    TempData["SuccessMessage"] = "Teacher has been deleted successfully.";
                    return RedirectToPage("ViewAllTeachers");
                }
                else
                {
                    TempData["ErrorMessage"] = "You cannot delete your own account.";
                    return RedirectToPage("ViewAllTeachers");
                }          
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the teacher: " + ex.Message;
                return RedirectToPage("ViewAllTeachers");
            }       
          
        }
    }
}
