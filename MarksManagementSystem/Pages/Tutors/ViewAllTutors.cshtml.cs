using MarksManagementSystem.Data;
using MarksManagementSystem.Data.Repositories;
using MarksManagementSystem.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
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
                   TutorId = t.Id,
                   TutorFullName = t.Name + " " + t.LastName,
                   TutorEmail = t.Email,
                   CourseLed = _courseTutorRepository.GetAll()
                       .Where(ct => ct.TutorId == t.Id && ct.IsUnitLeader == true)
                       .Select(ct => ct.Course.Name)
                       .SingleOrDefault(),

                   OtherCourses = string.Join(", ", _courseTutorRepository.GetAll()
                       .Where(ct => ct.TutorId == t.Id && ct.IsUnitLeader == false)
                       .Select(ct => ct.Course.Name)
                       .ToList())
               })
               .ToList();
        }

        public IActionResult OnPostDelete(int id)
        {
            if (_courseTutorRepository.GetAll().Where(ct => ct.TutorId==id && ct.IsUnitLeader==true).Any())
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
                    if (_tutorRepository.GetById(id).Email != (HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value))
                    {
                        _tutorRepository.Delete(id);
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

        public IActionResult OnPostEdit(int Id)
        {
            
            return RedirectToPage("EditTutor", new { Id });
        }
    }
}
