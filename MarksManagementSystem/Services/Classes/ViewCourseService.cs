using Azure.Core;
using MarksManagementSystem.Data.Models;
using MarksManagementSystem.Data.Repositories.Interfaces;
using MarksManagementSystem.Services.Interfaces;
using MarksManagementSystem.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace MarksManagementSystem.Services.Classes
{
    public class ViewCourseService : IViewCourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly ICourseTutorRepository _courseTutorRepository;
        private readonly ICourseStudentRepository _courseStudentRepository;
        private readonly IStudentRepository _studentRepository;

        public ViewCourseService(ICourseRepository courseRepository,
            ICourseTutorRepository courseTutorRepository,
            ICourseStudentRepository courseStudentRepository,
            IStudentRepository studentRepository)
        {
            _courseRepository = courseRepository ?? throw new ArgumentNullException(nameof(courseRepository));
            _courseStudentRepository = courseStudentRepository ?? throw new ArgumentNullException(nameof(courseStudentRepository));
            _courseTutorRepository = courseTutorRepository ?? throw new ArgumentNullException(nameof(courseTutorRepository));
            _studentRepository = studentRepository ?? throw new ArgumentNullException(nameof(studentRepository));
        }

        public Course GetCourseById(int courseId)
        {
            if (courseId <= 0) throw new ArgumentOutOfRangeException(nameof(courseId));
            return _courseRepository.GetById(courseId);
        }
        
        public Tutor GetUnitLeaderOfCourse(int courseId)
        {
            if (courseId <= 0) throw new ArgumentOutOfRangeException(nameof(courseId));
            return _courseTutorRepository.GetUnitLeaderOfCourse(courseId);
        }

        public void UpdateMarks(Course thisCourse, string? mark, int studentId)
        {
            if (thisCourse == null) throw new ArgumentNullException(nameof(thisCourse));
            if (studentId <= 0) throw new ArgumentOutOfRangeException(nameof(studentId));

            Student thisStudent = _studentRepository.GetById(Convert.ToInt32(studentId));
            var courseStudent = _courseStudentRepository.GetByIds(thisCourse.CourseId, thisStudent.StudentId);
            if (courseStudent != null)
            {
                if (string.IsNullOrEmpty(mark))
                    courseStudent.Mark = -1;
                else
                    courseStudent.Mark = Convert.ToInt32(mark);
                _courseStudentRepository.Update(courseStudent);
            }           
        }

        public List<ViewCourseViewModel> GetAllStudentEnrolled(int courseId)
        {
            if (courseId <= 0) throw new ArgumentOutOfRangeException(nameof(courseId));

            return _courseStudentRepository.GetAllByCourseId(courseId)
                .Select(s => new ViewCourseViewModel
                {
                    StudentId = s.StudentId.ToString(),
                    StudentFullName = s.Student.StudentFirstName + " " + s.Student.StudentLastName,
                    StudentEmail = s.Student.StudentEmail,
                    StudentDateOfBirth = s.Student.StudentDateOfBirth.Date.ToString("d"),
                    StudentMark = s.Mark.ToString()
                })
                .ToList();
        }
    }
}
