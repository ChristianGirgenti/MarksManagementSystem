using MarksManagementSystem.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace MarksManagementSystem.Data.Repositories
{
    public class CourseTutorRepository : ICourseTutorRepository
    {
        private MarksManagementContext marksManagementContext;

        public CourseTutorRepository(MarksManagementContext context)
        {
            marksManagementContext = context;
        }
        public void Add(CourseTutor courseTutor)
        {
            marksManagementContext.CourseTutors.Add(courseTutor);
            marksManagementContext.SaveChanges();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<CourseTutor> GetAll()
        {
            return marksManagementContext.CourseTutors
                .Include(ct => ct.Tutor)
                .Include(ct => ct.Course)
                .ToList();  
        }

        public CourseTutor GetById(int id)
        {
            return marksManagementContext.CourseTutors.FirstOrDefault(ct => ct.Id == id); 
        }

        public void Update(CourseTutor courseTutor)
        {
            marksManagementContext.Entry(courseTutor).State = EntityState.Modified;
            marksManagementContext.SaveChanges();
        }
    }
}
