using MarksManagementSystem.Data.Repositories;
using MarksManagementSystem.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MarksManagementSystem.Pages.Students
{
    [Authorize(Policy = "Admin")]
    public class ViewAllStudentsModel : PageModel
    {
        private readonly IStudentRepository _studentRepository;
        private readonly ICourseStudentRepository _courseStudentRepository;
        public List<ViewAllStudentsViewModel> AllStudentsViewModel { get; set; } = new List<ViewAllStudentsViewModel>();

        public ViewAllStudentsModel(IStudentRepository studentRepository, ICourseStudentRepository courseStudentRepository)
        {
            _studentRepository = studentRepository;
            _courseStudentRepository = courseStudentRepository;
        }
        public void OnGet()
        {
            AllStudentsViewModel = _studentRepository.GetAll()
               .Select(s => new ViewAllStudentsViewModel
               {
                   StudentId = s.StudentId,
                   StudentFullName = s.StudentFirstName + " " + s.StudentLastName,
                   StudentEmail = s.StudentEmail,
                   StudentDateOfBirth = s.StudentDateOfBirth.Date.ToString("d"),
                   StudentEnrolledCourses = string.Join(", ", _courseStudentRepository.GetEnrolledCoursesNameByStudentId(s.StudentId))
               })
               .ToList();
        }

        public IActionResult OnPostDelete(int studentId)
        {
            try
            { 
                _studentRepository.Delete(studentId);
                TempData["SuccessMessage"] = "The student has been deleted successfully.";
                return RedirectToPage("ViewAllStudents");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the student: " + ex.Message;
                return RedirectToPage("ViewAllStudents");
            }
        }
    }
}
