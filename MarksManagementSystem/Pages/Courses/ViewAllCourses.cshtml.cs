using MarksManagementSystem.Data;
using MarksManagementSystem.Data.Models;
using MarksManagementSystem.Helpers;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MarksManagementSystem.Pages.Courses
{
    public class ViewAllCoursesModel : PageModel
    {
        private readonly MarksManagementContext marksManagementContext;
        public List<CourseView>? AllCoursesWithTeacher { get; set; }

        public ViewAllCoursesModel(MarksManagementContext context)
        {
            marksManagementContext = context;
        }

        public void OnGet()
        {
            AllCoursesWithTeacher = marksManagementContext.Courses
                .Select(c => new CourseView
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
