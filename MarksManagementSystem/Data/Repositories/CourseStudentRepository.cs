using MarksManagementSystem.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace MarksManagementSystem.Data.Repositories
{
    public class CourseStudentRepository : ICourseStudentRepository
    {
        private readonly MarksManagementContext marksManagementContext;

        public CourseStudentRepository(MarksManagementContext context)
        {
            marksManagementContext = context;
        }
        public void Add(CourseStudent courseStudent)
        {
            if (courseStudent == null) throw new ArgumentNullException(nameof(courseStudent));
            marksManagementContext.CourseStudent.Add(courseStudent);
            marksManagementContext.SaveChanges();
        }

        public List<CourseStudent> GetAll()
        {
            return marksManagementContext.CourseStudent
                .Include(cs => cs.Student)
                .Include(cs => cs.Course)
                .ToList();
        }

        public CourseStudent GetByIds(int courseId, int studentId)
        {
            if (courseId <= 0) throw new ArgumentOutOfRangeException(nameof(courseId));
            if (studentId <= 0) throw new ArgumentOutOfRangeException(nameof(studentId));

            var courseStudents = marksManagementContext.CourseStudent;
            var courseStudentByIds = courseStudents.FirstOrDefault(ct => ct.CourseId == courseId && ct.StudentId == studentId);

            if (courseStudentByIds == null) throw new ArgumentNullException(nameof(courseStudentByIds));

            return courseStudentByIds;
        }

        public void Update(CourseStudent courseStudent)
        {
            if (courseStudent == null) throw new ArgumentNullException(nameof(courseStudent));
            marksManagementContext.Entry(courseStudent).State = EntityState.Modified;
            marksManagementContext.SaveChanges();
        }
    }
}
