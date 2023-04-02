using MarksManagementSystem.Data.Models;
using MarksManagementSystem.ViewModel;

namespace MarksManagementSystem.Services.Interfaces
{
    public interface IViewCourseService
    {
        public Course GetCourseById(int courseId);
        public void UpdateMarks(Course thisCourse, string? mark, int studentId);
        public List<ViewCourseViewModel> GetAllStudentEnrolled(int courseId);
        public Tutor GetUnitLeaderOfCourse(int courseId);
    }
}
