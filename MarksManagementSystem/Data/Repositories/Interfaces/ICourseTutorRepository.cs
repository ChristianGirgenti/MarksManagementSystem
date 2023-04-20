using MarksManagementSystem.Data.Models;

namespace MarksManagementSystem.Data.Repositories.Interfaces
{
    public interface ICourseTutorRepository
    {
        public void Add(CourseTutor courseTutor);
        public void Update(CourseTutor courseTutor);
        public CourseTutor GetByIds(int courseId, int tutorId);
        public List<CourseTutor> GetAll();
        public void DeleteUnitLeaderRelationshipByCourseId(int courseId);
        public void DeleteAllOtherTutorsByCourseId(int courseId);
        public List<CourseTutor> GetAllByTutorId(int tutorId);
        public Tutor GetUnitLeaderByCourseId(int courseId);
        public List<string> GetOtherTutorsToStringByCourseId(int courseId);
    }
}
