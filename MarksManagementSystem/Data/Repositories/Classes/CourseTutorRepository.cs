using MarksManagementSystem.Data.Models;
using MarksManagementSystem.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MarksManagementSystem.Data.Repositories.Classes
{
    public class CourseTutorRepository : ICourseTutorRepository
    {
        private readonly MarksManagementContext marksManagementContext;

        public CourseTutorRepository(MarksManagementContext context)
        {
            marksManagementContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void Add(CourseTutor courseTutor)
        {
            if (courseTutor == null) throw new ArgumentNullException(nameof(courseTutor));
            marksManagementContext.CourseTutor.Add(courseTutor);
            marksManagementContext.SaveChanges();
        }

        public void DeleteAllOtherTutorsByCourseId(int courseId)
        {

            if (courseId <= 0) throw new ArgumentOutOfRangeException(nameof(courseId));
            var deleteAllOtherTutors = marksManagementContext.CourseTutor.Where(ct => ct.CourseId == courseId && ct.IsUnitLeader == false);
            marksManagementContext.CourseTutor.RemoveRange(deleteAllOtherTutors);
            marksManagementContext.SaveChanges();
        }

        public void DeleteUnitLeaderRelationshipByCourseId(int courseId)
        {
            if (courseId <= 0) throw new ArgumentOutOfRangeException(nameof(courseId));
            var unitLeaderRelationshipToRemove = marksManagementContext.CourseTutor.Where(ct => ct.CourseId == courseId && ct.IsUnitLeader == true).FirstOrDefault();
            if (unitLeaderRelationshipToRemove == null) throw new ArgumentNullException(nameof(unitLeaderRelationshipToRemove));

            marksManagementContext.CourseTutor.Remove(unitLeaderRelationshipToRemove);
            marksManagementContext.SaveChanges();
        }

        public List<CourseTutor> GetAll()
        {
            return marksManagementContext.CourseTutor
                .Include(ct => ct.Tutor)
                .Include(ct => ct.Course)
                .ToList();
        }

        public List<CourseTutor> GetAllByTutorId(int tutorId)
        {
            if (tutorId <= 0) throw new ArgumentOutOfRangeException(nameof(tutorId));

            var courseTutors = marksManagementContext.CourseTutor;
            var courseTutorsByTutorId = courseTutors.Where(ct => ct.TutorId == tutorId)
                .Include(ct => ct.Tutor)
                .Include(ct => ct.Course)
                .ToList();
            return courseTutorsByTutorId;
        }

        public CourseTutor GetByIds(int courseId, int tutorId)
        {
            if (courseId <= 0) throw new ArgumentOutOfRangeException(nameof(courseId));
            if (tutorId <= 0) throw new ArgumentOutOfRangeException(nameof(tutorId));

            var courseTutors = marksManagementContext.CourseTutor;
            var courseTutorByIds = courseTutors.FirstOrDefault(ct => ct.CourseId == courseId && ct.TutorId == tutorId);

            if (courseTutorByIds == null) throw new ArgumentNullException(nameof(courseTutorByIds));

            return courseTutorByIds;
        }

        public void Update(CourseTutor courseTutor)
        {
            if (courseTutor == null) throw new ArgumentNullException(nameof(courseTutor));
            marksManagementContext.Entry(courseTutor).State = EntityState.Modified;
            marksManagementContext.SaveChanges();
        }

        public Tutor GetUnitLeaderByCourseId(int courseId)
        {
            if (courseId <= 0) throw new ArgumentOutOfRangeException(nameof(courseId));
            return marksManagementContext.CourseTutor.Where(ct => ct.CourseId == courseId && ct.IsUnitLeader == true)
                .Select(ct => ct.Tutor).First();
        }

        public List<string> GetOtherTutorsToStringByCourseId(int courseId)
        {
            if (courseId <= 0) throw new ArgumentOutOfRangeException(nameof(courseId));
            return marksManagementContext.CourseTutor.Where(ct => ct.CourseId == courseId && ct.IsUnitLeader == false).Select(ct => ct.Tutor.ToString()).ToList();
        }


    }
}
