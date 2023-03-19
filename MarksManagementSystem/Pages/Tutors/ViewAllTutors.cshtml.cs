using MarksManagementSystem.Data.Repositories;
using MarksManagementSystem.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace MarksManagementSystem.Pages.Tutors
{
    public class ViewAllTutorsModel : PageModel
    {
        private readonly ITutorRepository _tutorRepository;
        private readonly ICourseTutorRepository _courseTutorRepository;
        public List<ViewAllTutorsViewModel>? AllTutorsViewModel { get; set; }

        public ViewAllTutorsModel(ITutorRepository tutorRepository, ICourseTutorRepository courseTutorRepository)
        {
            _tutorRepository = tutorRepository;
            _courseTutorRepository = courseTutorRepository;
        }
        public void OnGet()
        {
            AllTutorsViewModel = _tutorRepository.GetAll()
               .Select(t => new ViewAllTutorsViewModel
               {
                   TutorId = t.TutorId,
                   TutorFullName = t.TutorFirstName + " " + t.TutorLastName,
                   TutorEmail = t.TutorEmail,
                   TutorDateOfBirth = t.TutorDateOfBirth.Date.ToString("d"),
                   CourseLed = _courseTutorRepository.GetAll()
                       .Where(ct => ct.TutorId == t.TutorId && ct.IsUnitLeader)
                       .Select(ct => ct.Course.CourseName)
                       .SingleOrDefault(),

                   OtherCourses = string.Join(", ", _courseTutorRepository.GetAll()
                       .Where(ct => ct.TutorId == t.TutorId && ct.IsUnitLeader == false)
                       .Select(ct => ct.Course.CourseName)
                       .ToList())
               })
               .ToList();
        }

        public IActionResult OnPostDelete(int tutorId)
        {
            if (_courseTutorRepository.GetAll().Where(ct => ct.TutorId == tutorId && ct.IsUnitLeader==true).Any())
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
                    if (_tutorRepository.GetById(tutorId).TutorEmail != (HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value))
                    {
                        _tutorRepository.Delete(tutorId);
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

        public IActionResult OnPostEdit(int tutorId)
        {
            
            return RedirectToPage("EditTutor", new { tutorId });
        }
    }
}
