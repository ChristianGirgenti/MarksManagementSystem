using MarksManagementSystem.Data.Repositories;
using MarksManagementSystem.Helpers;
using MarksManagementSystem.ViewModel;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MarksManagementSystem.Pages.Courses
{
    public class ViewAllCoursesModel : PageModel
    {
        private readonly ICourseRepository courseRepository;
        private readonly ICourseTeacherRepository courseTeacherRepository;
        public List<ViewAllCoursesViewModel>? AllCoursesWithTeacher { get; set; }

        public ViewAllCoursesModel(ICourseRepository courseRepository, ICourseTeacherRepository courseTeacherRepository)
        {
            this.courseRepository = courseRepository;
            this.courseTeacherRepository = courseTeacherRepository; 
        }

        public void OnGet()
        {
            var x = courseTeacherRepository.GetAll();
            AllCoursesWithTeacher = courseRepository.GetAll()
                .Select(c => new ViewAllCoursesViewModel
                {
                    CourseName = c.Name,
                    CourseCredits = c.Credits,
                    HeadTeacher = courseTeacherRepository.GetAll()
                        .Where(ct => ct.CourseId == c.Id && ct.IsHeadTeacher == true)
                        .Select(ct => ct.Teacher.ToString())
                        .SingleOrDefault(),

                    OtherTeachers = string.Join(", ", courseTeacherRepository.GetAll()
                        .Where(ct => ct.CourseId == c.Id && ct.IsHeadTeacher == false)     
                        .Select(ct => ct.Teacher.ToString())
                        .ToList())
                })
                .ToList();
        }

       
    }
}
