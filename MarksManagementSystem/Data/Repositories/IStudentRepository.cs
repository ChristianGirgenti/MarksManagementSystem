using MarksManagementSystem.Data.Models;

namespace MarksManagementSystem.Data.Repositories
{
    public interface IStudentRepository
    {
        public void Add(Student student);
        public void Update(Student student);
        public Student GetById(int studentId);
        public void Delete(int studentId);
        public List<Student> GetAll();
        public void UpdatePasswordByStudentId(int studentId, string newPassword);
    }
}
