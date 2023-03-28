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
            if (tutor == null) throw new ArgumentNullException(nameof(tutor));
            marksManagementContext.Tutor.Add(tutor);
            marksManagementContext.SaveChanges();   
        }

        public void Delete(int tutorId)
        {
            if (tutorId <= 0) throw new ArgumentOutOfRangeException(nameof(tutorId));
            var deleteTutor = marksManagementContext.Tutor.FirstOrDefault(c => c.TutorId == tutorId);
            if (deleteTutor == null) throw new ArgumentNullException(nameof(deleteTutor));

            marksManagementContext.Tutor.Remove(deleteTutor);
            marksManagementContext.SaveChanges();
        }

        public List<Tutor> GetAll()
        {
            return marksManagementContext.Tutor.ToList();
        }

        public Tutor GetById(int tutorId)
        {
            if (tutorId <= 0) throw new ArgumentOutOfRangeException(nameof(tutorId));
            var tutor = marksManagementContext.Tutor.FirstOrDefault(t => t.TutorId == tutorId);
            if (tutor == null) throw new ArgumentNullException(nameof(tutor));
            return tutor; 
        }

        public void Update(Tutor tutor)
        {
            if (tutor == null) throw new ArgumentNullException(nameof(tutor));
            marksManagementContext.Entry(tutor).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            marksManagementContext.SaveChanges();
        }

        public void UpdatePasswordByTutorId(int tutorId, string newPassword)
        {
            if (tutorId <= 0) throw new ArgumentOutOfRangeException(nameof(tutorId));
            if (string.IsNullOrEmpty(newPassword)) throw new ArgumentNullException(nameof(newPassword));
            var tutor = marksManagementContext.Tutor.FirstOrDefault(t => t.TutorId == tutorId);
            if (tutor == null) throw new ArgumentNullException(nameof(tutor));
            tutor.TutorPassword = newPassword;
            marksManagementContext.Entry(tutor).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            marksManagementContext.SaveChanges();
        }
    }
}
