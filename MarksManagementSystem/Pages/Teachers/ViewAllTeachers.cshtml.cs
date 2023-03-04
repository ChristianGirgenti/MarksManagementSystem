using MarksManagementSystem.Data;
using MarksManagementSystem.Data.Repositories;
using MarksManagementSystem.ViewModel;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MarksManagementSystem.Pages.Teachers
{
    public class ViewAllTeachersModel : PageModel
    {
        private readonly ITeacherRepository teacherRepository;
        private readonly ICourseTeacherRepository courseTeacherRepository;
        public List<ViewAllTeachersViewModel>? AllTeachersViewModel { get; set; }

        public ViewAllTeachersModel(ITeacherRepository teacherRepository, ICourseTeacherRepository courseTeacherRepository)
        {
            this.teacherRepository = teacherRepository;
            this.courseTeacherRepository = courseTeacherRepository;
        }
        public void OnGet()
        {
            AllTeachersViewModel = teacherRepository.GetAll()
               .Select(t => new ViewAllTeachersViewModel
               {
                   TeacherFullName = t.Name + " " + t.LastName,
                   TeacherEmail = t.Email,
                   CourseLed = courseTeacherRepository.GetAll()
                       .Where(ct => ct.TeacherId == t.Id && ct.IsHeadTeacher == true)
                       .Select(ct => ct.Course.Name)
                       .SingleOrDefault(),

                   OtherCourses = string.Join(", ", courseTeacherRepository.GetAll()
                       .Where(ct => ct.TeacherId == t.Id && ct.IsHeadTeacher == false)
                       .Select(ct => ct.Course.Name)
                       .ToList())
               })
               .ToList();
        }
    }
}
