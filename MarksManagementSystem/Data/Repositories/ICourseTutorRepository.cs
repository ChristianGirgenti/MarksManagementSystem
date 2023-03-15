using MarksManagementSystem.Data.Models;

namespace MarksManagementSystem.Data.Repositories
{
    public interface ICourseTutorRepository
    {
        public void Add(CourseTutor course);
        public void Update(CourseTutor course);
        public CourseTutor GetByIds(int courseId, int tutorId);
        public List<CourseTutor> GetAll();
        public void DeleteCourseUnitLeaderRelationshipByCourseId(int courseId);
        public void DeleteAllOtherTutorsInACourse(int courseId);
    }
}
