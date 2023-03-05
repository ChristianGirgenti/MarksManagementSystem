using MarksManagementSystem.Data.Models;

namespace MarksManagementSystem.Data.Repositories
{
    public interface ITeacherRepository
    {
        public void Add(Teacher course);
        public void Update(Teacher course);
        public Teacher GetById(int id);
        public void Delete(int id);
        public List<Teacher> GetAll();
    }
}
