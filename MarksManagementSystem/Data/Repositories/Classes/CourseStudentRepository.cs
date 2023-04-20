using MarksManagementSystem.Data.Models;
using MarksManagementSystem.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MarksManagementSystem.Data.Repositories.Classes
{
    public class CourseStudentRepository : ICourseStudentRepository
    {
        private readonly MarksManagementContext marksManagementContext;

        public CourseStudentRepository(MarksManagementContext context)
        {
            marksManagementContext = context ?? throw new ArgumentNullException(nameof(context));
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

        public List<CourseStudent> GetAllByCourseId(int courseId)
        {
            if (courseId <= 0) throw new ArgumentOutOfRangeException(nameof(courseId));

            var courseStudents = marksManagementContext.CourseStudent;
            var courseStudentsByCourseId = courseStudents.Where(cs => cs.CourseId == courseId)
                .Include(cs => cs.Student)
                .Include(cs => cs.Course)
                .ToList();

            return courseStudentsByCourseId;
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

        public List<CourseStudent> GetAllByStudentId(int studentId)
        {
            if (studentId <= 0) throw new ArgumentOutOfRangeException(nameof(studentId));

            var courseStudents = marksManagementContext.CourseStudent;
            var courseStudentsByStudentId = courseStudents.Where(cs => cs.StudentId == studentId)
                .Include(cs => cs.Student)
                .Include(cs => cs.Course)
                .ToList();
            return courseStudentsByStudentId;
        }

        public void DeleteCourseStudentRelationshipByIds(int courseId, int studentId)
        {
            if (courseId <= 0) throw new ArgumentOutOfRangeException(nameof(courseId));
            if (studentId <= 0) throw new ArgumentOutOfRangeException(nameof(studentId));
            var courseStudentRelationshipToRemove = marksManagementContext.CourseStudent
                .Where(cs => cs.CourseId == courseId && cs.StudentId == studentId).FirstOrDefault();
            if (courseStudentRelationshipToRemove == null) throw new ArgumentNullException(nameof(courseStudentRelationshipToRemove));

            marksManagementContext.CourseStudent.Remove(courseStudentRelationshipToRemove);
            marksManagementContext.SaveChanges();
        }

        public List<string> GetEnrolledCoursesNameByStudentId(int studentId)
        {
            if (studentId <= 0) throw new ArgumentOutOfRangeException(nameof(studentId));
            return marksManagementContext.CourseStudent.Where(cs => cs.StudentId == studentId).Select(cs => cs.Course.CourseName).ToList();
        }
    }
}
