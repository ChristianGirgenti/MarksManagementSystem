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
            throw new NotImplementedException();
        }

        public List<Teacher> GetAll()
        {
            return marksManagementContext.Teachers.ToList();
        }

        public void GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(Teacher course)
        {
            throw new NotImplementedException();
        }
    }
}
