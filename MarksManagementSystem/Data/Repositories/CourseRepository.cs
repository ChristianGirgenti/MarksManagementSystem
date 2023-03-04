using MarksManagementSystem.Data.Models;

namespace MarksManagementSystem.Data.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private MarksManagementContext marksManagementContext;

        public CourseRepository(MarksManagementContext context)
        {
            marksManagementContext = context;
        }

        public void Add(Course course)
        {
            marksManagementContext.Courses.Add(course);
            marksManagementContext.SaveChanges();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<Course> GetAll()
        {
            return marksManagementContext.Courses.ToList();
        }

        public void GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(Course course)
        {
            throw new NotImplementedException();
        }
    }
}
