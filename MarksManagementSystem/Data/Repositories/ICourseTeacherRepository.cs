using MarksManagementSystem.Data.Models;

namespace MarksManagementSystem.Data.Repositories
{
    public interface ICourseTeacherRepository
    {
        public void Add(CourseTeacher course);
        public void Update(CourseTeacher course);
        public void GetById(int id);
        public void Delete(int id);
        public List<CourseTeacher> GetAll();
    }
}
