using MarksManagementSystem.Data.Models;

namespace MarksManagementSystem.Data.Repositories
{
    public interface ICourseRepository
    {
        public void Add(Course course);
        public void Update(Course course);
        public void GetById(int id);
        public void Delete(int id);
        public List<Course> GetAll();

    }
}
