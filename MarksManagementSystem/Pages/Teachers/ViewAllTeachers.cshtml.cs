using MarksManagementSystem.Data;
using MarksManagementSystem.ViewModel;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MarksManagementSystem.Pages.Teachers
{
    public class ViewAllTeachersModel : PageModel
    {
        private readonly MarksManagementContext marksManagementContext;
        public List<ViewAllTeachersViewModel>? AllTeachersViewModel { get; set; }

        public ViewAllTeachersModel(MarksManagementContext context)
        {
            marksManagementContext = context;
        }
        public void OnGet()
        {
            AllTeachersViewModel = marksManagementContext.Teachers
               .Select(t => new ViewAllTeachersViewModel
               {
                   TeacherFullName = t.Name + " " + t.LastName,
                   TeacherEmail = t.Email,
                   CourseLed = marksManagementContext.CourseTeachers
                       .Where(ct => ct.TeacherId == t.Id && ct.IsHeadTeacher == true)
                       .Select(ct => ct.Course.Name)
                       .SingleOrDefault(),

                   OtherCourses = string.Join(", ", marksManagementContext.CourseTeachers
                       .Where(ct => ct.TeacherId == t.Id && ct.IsHeadTeacher == false)
                       .Select(ct => ct.Course.Name)
                       .ToList())
               })
               .ToList();
        }
    }
}
