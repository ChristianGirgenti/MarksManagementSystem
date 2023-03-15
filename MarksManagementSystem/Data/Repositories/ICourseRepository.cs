using MarksManagementSystem.Data.Models;

namespace MarksManagementSystem.Data.Repositories
{
    public interface ICourseRepository
    {
        public void Add(Course course);
        public void Update(Course course);
        public Course GetById(int courseId);
        public void Delete(int courseId);
        public List<Course> GetAll();

    }
}
