using MarksManagementSystem.Services.Interfaces;
using MarksManagementSystem.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MarksManagementSystem.Pages.Tutors
{
    [Authorize(Policy = "Admin")]
    public class ViewAllTutorsModel : PageModel
    {
        private readonly IViewAllTutorsService _viewAllTutorsService;
        public List<ViewAllTutorsViewModel> AllTutorsViewModel { get; set; } = new List<ViewAllTutorsViewModel>();

        public ViewAllTutorsModel(IViewAllTutorsService viewAllTutorsService)
        {
            _viewAllTutorsService = viewAllTutorsService ?? throw new ArgumentNullException(nameof(viewAllTutorsService));
        }
        public void OnGet()
        {
            AllTutorsViewModel = _viewAllTutorsService.GetAllTutorsViewModel();
        }

        public IActionResult OnPostDelete(int tutorId)
        {
            if (_viewAllTutorsService.IsTutorUnitLeader(tutorId))
            {
                {
                    TempData["ErrorMessage"] = "You cannot delete this tutor because is unit leader of a course. Remove the link between unit leader and course.";
                    return RedirectToPage("ViewAllTutors");
                }
            }
            else
            {
                try
                {
                    if (_viewAllTutorsService.DeleteTutor(tutorId, HttpContext.User.Claims))
                    {
                        TempData["SuccessMessage"] = "The tutor has been deleted successfully.";
                        return RedirectToPage("ViewAllTutors");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "You cannot delete your own account.";
                        return RedirectToPage("ViewAllTutors");
                    }
                }
                catch (Exception ex)
                {
                    TempData["ErrorMessage"] = "An error occurred while deleting the tutor: " + ex.Message;
                    return RedirectToPage("ViewAllTutors");
                }
            }     
        }
    }
}
