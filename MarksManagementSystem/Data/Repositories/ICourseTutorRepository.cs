using MarksManagementSystem.Data.Models;

namespace MarksManagementSystem.Data.Repositories
{
    public interface ICourseTutorRepository
    {
        public void Add(CourseTutor courseTutor);
        public void Update(CourseTutor courseTutor);
        public CourseTutor GetByIds(int courseId, int tutorId);
        public List<CourseTutor> GetAll();
        public void DeleteCourseUnitLeaderRelationshipByCourseId(int courseId);
        public void DeleteAllOtherTutorsInACourse(int courseId);
    }
}
