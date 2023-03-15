using MarksManagementSystem.Data.Repositories;
using MarksManagementSystem.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

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
                    CourseId = c.CourseId,
                    CourseName = c.CourseName,
                    CourseCredits = c.CourseCredits,

                    UnitLeader = _courseTutorRepository.GetAll()
                        .Where(ct => ct.CourseId == c.CourseId && ct.IsUnitLeader == true)
                        .Select(ct => ct.Tutor.ToString())
                        .SingleOrDefault(),

                    OtherTutors = string.Join(", ", _courseTutorRepository.GetAll()
                        .Where(ct => ct.CourseId == c.CourseId && ct.IsUnitLeader == false)
                        .Select(ct => ct.Tutor.ToString())
                        .ToList())
                })
                .ToList();
        }

        public IActionResult OnPostDelete(int courseId)
        {
            if (courseId <= 0) throw new ArgumentNullException(nameof(courseId));
            try
            {
                _courseRepository.Delete(courseId);
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
