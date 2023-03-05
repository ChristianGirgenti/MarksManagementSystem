using MarksManagementSystem.Data.Models;

namespace MarksManagementSystem.Data.Repositories
{
    public class TeacherRepository : ITeacherRepository
    {
        private MarksManagementContext marksManagementContext;

        public TeacherRepository(MarksManagementContext context)
        {
            marksManagementContext = context;
        }

        public void Add(Teacher teacher)
        {
            marksManagementContext.Teachers.Add(teacher);
            marksManagementContext.SaveChanges();   
        }

        public void Delete(int id)
        {
            var deleteTeacher = marksManagementContext.Teachers.FirstOrDefault(c => c.Id == id);
            if (deleteTeacher == null) throw new ArgumentNullException(nameof(id));

            marksManagementContext.Teachers.Remove(deleteTeacher);
            marksManagementContext.SaveChanges();
        }

        public List<Teacher> GetAll()
        {
            return marksManagementContext.Teachers.ToList();
        }

        public Teacher GetById(int id)
        {
            
            return marksManagementContext.Teachers.FirstOrDefault(t => t.Id == id); 
        }

        public void Update(Teacher course)
        {
            throw new NotImplementedException();
        }
    }
}
