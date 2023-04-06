using MarksManagementSystem.Services.Interfaces;
using MarksManagementSystem.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MarksManagementSystem.Pages.Courses
{
    [Authorize(Policy = "Admin")]
    public class ViewAllCoursesModel : PageModel
    {
        private readonly IViewAllCoursesService _viewAllCoursesService;
        public List<ViewAllCoursesViewModel> AllCoursesWithTutor { get; set; } = new List<ViewAllCoursesViewModel>();
        public ViewAllCoursesModel(IViewAllCoursesService viewAllCoursesService)
        {
            if (viewAllCoursesService == null) throw new ArgumentOutOfRangeException(nameof(viewAllCoursesService));
            _viewAllCoursesService = viewAllCoursesService; 
        }

        public void OnGet()
        { 
            AllCoursesWithTutor = _viewAllCoursesService.GetAllCoursesWithTutors();
        }

        public IActionResult OnPostDelete(int courseId)
        {
            if (courseId <= 0) throw new ArgumentNullException(nameof(courseId));
            try
            {
                _viewAllCoursesService.DeleteCourse(courseId);
                TempData["SuccessMessage"] = "The course has been deleted successfully.";
                return RedirectToPage("ViewAllCourses"); 
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the course: " + ex.Message;
                return RedirectToPage("ViewAllCourses");
            }
        }

        public IActionResult OnPostEdit(int courseId)
        {
            if (courseId <= 0) throw new ArgumentNullException(nameof(courseId));
            return RedirectToPage("EditCourse", new { courseId });
        }


    }
}
