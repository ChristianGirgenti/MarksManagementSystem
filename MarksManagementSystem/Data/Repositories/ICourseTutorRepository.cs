using MarksManagementSystem.Data.Models;

namespace MarksManagementSystem.Data.Repositories
{
    public interface ICourseTutorRepository
    {
        public void Add(CourseTutor course);
        public void Update(CourseTutor course);
        public CourseTutor GetById(int id);
        public void Delete(int id);
        public List<CourseTutor> GetAll();
        public void DeleteAllOtherTutorsInACourse(int courseId);
    }
}
