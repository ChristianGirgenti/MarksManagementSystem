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
        private readonly ICourseTeacherRepository _courseTeacherRepository;
        public List<ViewAllCoursesViewModel>? AllCoursesWithTeacher { get; set; }
        public ViewAllCoursesModel(ICourseRepository courseRepository, ICourseTeacherRepository courseTeacherRepository)
        {
            _courseRepository = courseRepository;
            _courseTeacherRepository = courseTeacherRepository; 
        }

        public void OnGet()
        {
            AllCoursesWithTeacher = _courseRepository.GetAll()
                .Select(c => new ViewAllCoursesViewModel
                {
                    CourseId = c.Id,
                    CourseName = c.Name,
                    CourseCredits = c.Credits,
                    UnitLeader = _courseTeacherRepository.GetAll()
                        .Where(ct => ct.CourseId == c.Id && ct.IsUnitLeader == true)
                        .Select(ct => ct.Teacher.ToString())
                        .SingleOrDefault(),

                    OtherTeachers = string.Join(", ", _courseTeacherRepository.GetAll()
                        .Where(ct => ct.CourseId == c.Id && ct.IsUnitLeader == false)     
                        .Select(ct => ct.Teacher.ToString())
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
