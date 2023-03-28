using MarksManagementSystem.Data.Models;

namespace MarksManagementSystem.Data.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly MarksManagementContext marksManagementContext;

        public CourseRepository(MarksManagementContext context)
        {
            marksManagementContext = context;
        }

        public void Add(Course course)
        {
            if (course == null) throw new ArgumentNullException(nameof(course));
            marksManagementContext.Course.Add(course);
            marksManagementContext.SaveChanges();
        }

        public void Delete(int courseId)
        {
            if (courseId <= 0) throw new ArgumentOutOfRangeException(nameof(courseId));
            var deleteCourse = marksManagementContext.Course.FirstOrDefault(c => c.CourseId == courseId);
            if (deleteCourse == null) throw new ArgumentNullException(nameof(deleteCourse));

            marksManagementContext.Course.Remove(deleteCourse);
            marksManagementContext.SaveChanges();
            
        }

        public List<Course> GetAll()
        {
            return marksManagementContext.Course.ToList();
        }

        public Course GetById(int courseId)
        {
            if (courseId <= 0) throw new ArgumentOutOfRangeException(nameof(courseId));
            var courseById = marksManagementContext.Course.FirstOrDefault(c => c.CourseId == courseId);
            if (courseById == null) throw new ArgumentNullException(nameof(courseById));
            return courseById;
        }

        public void Update(Course course)
        {
            if (course == null) throw new ArgumentNullException(nameof(course));
            marksManagementContext.Entry(course).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            marksManagementContext.SaveChanges();
        }
    }
}
