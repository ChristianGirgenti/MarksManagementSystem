using MarksManagementSystem.Services.Interfaces;
using MarksManagementSystem.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MarksManagementSystem.Pages.Students
{
    [Authorize(Policy = "Admin")]
    public class ViewAllStudentsModel : PageModel
    {
        private readonly IViewAllStudentsService _viewAllStudentsService;

        public List<ViewAllStudentsViewModel> AllStudentsViewModel { get; set; } = new List<ViewAllStudentsViewModel>();

        public ViewAllStudentsModel(IViewAllStudentsService viewAllStudentsService)
        {
            _viewAllStudentsService = viewAllStudentsService;
        }
        public void OnGet()
        {
            AllStudentsViewModel = _viewAllStudentsService.GetAllStudentsViewModel();
        }

        public IActionResult OnPostDelete(int studentId)
        {
            try
            { 
                _viewAllStudentsService.DeleteStudent(studentId);
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
