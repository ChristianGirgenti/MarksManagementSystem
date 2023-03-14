using MarksManagementSystem.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace MarksManagementSystem.Data.Repositories
{
    public class CourseTutorRepository : ICourseTutorRepository
    {
        private readonly MarksManagementContext marksManagementContext;

        public CourseTutorRepository(MarksManagementContext context)
        {
            marksManagementContext = context;
        }
        public void Add(CourseTutor courseTutor)
        {
            if (courseTutor == null) throw new ArgumentNullException(nameof(courseTutor));
            marksManagementContext.CourseTutors.Add(courseTutor);
            marksManagementContext.SaveChanges();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public void DeleteAllOtherTutorsInACourse(int courseId)
        {

            if (courseId <= 0) throw new ArgumentOutOfRangeException(nameof(courseId));
            var deleteAllOtherTutors = marksManagementContext.CourseTutors.Where(ct => ct.CourseId == courseId && ct.IsUnitLeader == false);
            if (deleteAllOtherTutors == null) throw new ArgumentNullException(nameof(courseId));

            marksManagementContext.CourseTutors.RemoveRange(deleteAllOtherTutors);
            marksManagementContext.SaveChanges();
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
            if (id <=0) throw new ArgumentOutOfRangeException(nameof(id));
            return marksManagementContext.CourseTutors.FirstOrDefault(ct => ct.Id == id); 
        }

        public void Update(CourseTutor courseTutor)
        {
            if (courseTutor == null) throw new ArgumentNullException(nameof(courseTutor));
            marksManagementContext.Entry(courseTutor).State = EntityState.Modified;
            marksManagementContext.SaveChanges();
        }
    }
}
