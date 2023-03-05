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

            var deleteCourse = marksManagementContext.Courses.FirstOrDefault(c => c.Id == id);
            if (deleteCourse == null) throw new ArgumentNullException(nameof(id));

            marksManagementContext.Courses.Remove(deleteCourse);
            marksManagementContext.SaveChanges();
            
        }

        public List<Course> GetAll()
        {
            return marksManagementContext.Courses.ToList();
        }

        public Course GetById(int id)
        {
            return marksManagementContext.Courses.FirstOrDefault(c => c.Id == id);
        }

        public void Update(Course course)
        {
            marksManagementContext.Entry(course).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            marksManagementContext.SaveChanges();
        }
    }
}
