using MarksManagementSystem.Data.Models;
using MarksManagementSystem.Data.Repositories.Interfaces;

namespace MarksManagementSystem.Data.Repositories.Classes
{
    public class StudentRepository : IStudentRepository
    {
        private readonly MarksManagementContext marksManagementContext;

        public StudentRepository(MarksManagementContext context)
        {
            marksManagementContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void Add(Student student)
        {
            if (student == null) throw new ArgumentNullException(nameof(student));
            marksManagementContext.Student.Add(student);
            marksManagementContext.SaveChanges();
        }

        public void Delete(int studentId)
        {
            if (studentId <= 0) throw new ArgumentOutOfRangeException(nameof(studentId));
            var deleteStudent = marksManagementContext.Student.FirstOrDefault(c => c.StudentId == studentId);
            if (deleteStudent == null) throw new ArgumentNullException(nameof(deleteStudent));

            marksManagementContext.Student.Remove(deleteStudent);
            marksManagementContext.SaveChanges();
        }

        public List<Student> GetAll()
        {
            return marksManagementContext.Student.ToList();
        }

        public Student GetById(int studentId)
        {
            if (studentId <= 0) throw new ArgumentOutOfRangeException(nameof(studentId));
            var student = marksManagementContext.Student.FirstOrDefault(s => s.StudentId == studentId);
            if (student == null) throw new ArgumentNullException(nameof(student));
            return student;
        }

        public void Update(Student student)
        {
            if (student == null) throw new ArgumentNullException(nameof(student));
            marksManagementContext.Entry(student).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            marksManagementContext.SaveChanges();
        }

        public void UpdatePasswordByStudentId(int studentId, string newPassword)
        {
            if (studentId <= 0) throw new ArgumentOutOfRangeException(nameof(studentId));
            if (string.IsNullOrEmpty(newPassword)) throw new ArgumentNullException(nameof(newPassword));
            var student = marksManagementContext.Student.FirstOrDefault(s => s.StudentId == studentId);
            if (student == null) throw new ArgumentNullException(nameof(student));
            student.StudentPassword = newPassword;
            marksManagementContext.Entry(student).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            marksManagementContext.SaveChanges();
        }
    }
}
