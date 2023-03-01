using MarksManagementSystem.Data;
using MarksManagementSystem.Helpers;
using MarksManagementSystem.ViewModel;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MarksManagementSystem.Pages.Courses
{
    public class ViewAllCoursesModel : PageModel
    {
        private readonly MarksManagementContext marksManagementContext;
        public List<ViewModel.ViewAllCoursesModel>? AllCoursesWithTeacher { get; set; }

        public ViewAllCoursesModel(MarksManagementContext context)
        {
            marksManagementContext = context;
        }

        public void OnGet()
        {
            AllCoursesWithTeacher = marksManagementContext.Courses
                .Select(c => new ViewAllCoursesModel
                {
                    CourseName = c.Name,
                    CourseCredits = c.Credits,
                    HeadTeacher = marksManagementContext.CourseTeachers
                        .Where(ct => ct.CourseId == c.Id && ct.IsHeadTeacher == true)
                        .Select(ct => ct.Teacher.ToString())
                        .SingleOrDefault(),

                    OtherTeachers = string.Join(", ", marksManagementContext.CourseTeachers
                        .Where(ct => ct.CourseId == c.Id && ct.IsHeadTeacher == false)     
                        .Select(ct => ct.Teacher.ToString())
                        .ToList())
                })
                .ToList();
        }

       
    }
}
