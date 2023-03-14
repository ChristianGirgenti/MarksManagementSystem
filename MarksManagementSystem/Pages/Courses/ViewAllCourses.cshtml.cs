using MarksManagementSystem.Data.Repositories;
using MarksManagementSystem.Helpers;
using MarksManagementSystem.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace MarksManagementSystem.Pages.Courses
{
    public class ViewAllCoursesModel : PageModel
    {
        private readonly ICourseRepository _courseRepository;
        private readonly ICourseTutorRepository _courseTutorRepository;
        public List<ViewAllCoursesViewModel>? AllCoursesWithTutor { get; set; }
        public ViewAllCoursesModel(ICourseRepository courseRepository, ICourseTutorRepository courseTutorRepository)
        {
            _courseRepository = courseRepository;
            _courseTutorRepository = courseTutorRepository; 
        }

        public void OnGet()
        {
            AllCoursesWithTutor = _courseRepository.GetAll()
                .Select(c => new ViewAllCoursesViewModel
                {
                    CourseId = c.Id,
                    CourseName = c.Name,
                    CourseCredits = c.Credits,
                    UnitLeader = _courseTutorRepository.GetAll()
                        .Where(ct => ct.CourseId == c.Id && ct.IsUnitLeader == true)
                        .Select(ct => ct.Tutor.ToString())
                        .SingleOrDefault(),

                    OtherTutors = string.Join(", ", _courseTutorRepository.GetAll()
                        .Where(ct => ct.CourseId == c.Id && ct.IsUnitLeader == false)     
                        .Select(ct => ct.Tutor.ToString())
                        .ToList())
                })
                .ToList();
        }

        public IActionResult OnPostDelete(int id)
        {
            if (id <= 0) throw new ArgumentNullException(nameof(id));
            try
            {
                _courseRepository.Delete(id);
                TempData["SuccessMessage"] = "The course has been deleted successfully.";
                return RedirectToPage("ViewAllCourses"); 
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the course: " + ex.Message;
                return RedirectToPage("ViewAllCourses");
            }
        }

        public IActionResult OnPostEdit(int Id)
        {
            if (Id <= 0) throw new ArgumentNullException(nameof(Id));
            return RedirectToPage("EditCourse", new { Id });
        }


    }
}
