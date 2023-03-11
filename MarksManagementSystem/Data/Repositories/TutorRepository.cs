using MarksManagementSystem.Data.Models;

namespace MarksManagementSystem.Data.Repositories
{
    public class TutorRepository : ITutorRepository
    {
        private readonly MarksManagementContext marksManagementContext;

        public TutorRepository(MarksManagementContext context)
        {
            marksManagementContext = context;
        }

        public void Add(Tutor tutor)
        {
            marksManagementContext.Tutors.Add(tutor);
            marksManagementContext.SaveChanges();   
        }

        public void Delete(int id)
        {
            var deleteTutor = marksManagementContext.Tutors.FirstOrDefault(c => c.Id == id);
            if (deleteTutor == null) throw new ArgumentNullException(nameof(id));

            marksManagementContext.Tutors.Remove(deleteTutor);
            marksManagementContext.SaveChanges();
        }

        public List<Tutor> GetAll()
        {
            return marksManagementContext.Tutors.ToList();
        }

        public Tutor GetById(int id)
        {
            
            return marksManagementContext.Tutors.FirstOrDefault(t => t.Id == id); 
        }

        public void Update(Tutor tutor)
        {
            marksManagementContext.Entry(tutor).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            marksManagementContext.SaveChanges();
        }
    }
}
