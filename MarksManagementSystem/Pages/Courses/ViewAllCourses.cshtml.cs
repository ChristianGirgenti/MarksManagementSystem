using MarksManagementSystem.Data;
using MarksManagementSystem.Data.Models;
using MarksManagementSystem.Helpers;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MarksManagementSystem.Pages.Courses
{
    public class ViewAllCoursesModel : PageModel
    {
        private MarksManagementContext marksManagementContext;
        public List<CourseView> AllCoursesWithTeacher { get; set; }

        public ViewAllCoursesModel(MarksManagementContext context)
        {
            marksManagementContext = context;
        }

        public void OnGet()
        {
            AllCoursesWithTeacher = marksManagementContext.Courses.Join(marksManagementContext.Teachers,
                c => c.HeadTeacherId,
                t => t.Id,
                (c, t) => new CourseView
                {
                    CourseName = c.Name,
                    CourseCredits = c.Credits,
                    HeadTeacherName = t.Name + " " + t.LastName
                }).ToList();
        }
    }
}
