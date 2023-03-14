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
            if (course == null) throw new ArgumentNullException(nameof(course));
            marksManagementContext.Courses.Add(course);
            marksManagementContext.SaveChanges();
        }

        public void Delete(int id)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));
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
            if (id<=0) throw new ArgumentOutOfRangeException(nameof(id));
            return marksManagementContext.Courses.FirstOrDefault(c => c.Id == id);
        }

        public void Update(Course course)
        {
            if (course == null) throw new ArgumentNullException(nameof(course));
            marksManagementContext.Entry(course).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            marksManagementContext.SaveChanges();
        }
    }
}
