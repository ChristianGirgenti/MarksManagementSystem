using MarksManagementSystem.Data.Models;

namespace MarksManagementSystem.Data.Repositories
{
    public interface ICourseStudentRepository
    {
        public void Add(CourseStudent courseStudent);
        public void Update(CourseStudent courseStudent);
        public CourseStudent GetByIds(int courseId, int studentId);
        public List<CourseStudent> GetAll();
        public List<CourseStudent> GetAllByStudentId(int studentId);

    }
}
