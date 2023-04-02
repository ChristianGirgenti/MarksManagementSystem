using Azure.Core;
using MarksManagementSystem.Data.Models;
using MarksManagementSystem.Data.Repositories;
using MarksManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MarksManagementSystem.Services.Classes
{
    public class CourseStudentManagementService : ICourseStudentManagementService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly ICourseStudentRepository _courseStudentRepository;

        public CourseStudentManagementService(ICourseRepository courseRepository, IStudentRepository studentRepository, ICourseStudentRepository courseStudentRepository)
        {
            _courseRepository = courseRepository;
            _studentRepository = studentRepository;
            _courseStudentRepository = courseStudentRepository;
        }

        public Course GetCourseById(int courseId)
        {
            if (courseId < 0) throw new ArgumentOutOfRangeException(nameof(courseId));
            return _courseRepository.GetById(courseId);
        }

        public List<CourseStudent> GetAllCurrentStudentsInTheCourse(int courseId)
        {
            if (courseId < 0) throw new ArgumentOutOfRangeException(nameof(courseId));
            return _courseStudentRepository.GetAllByCourseId(courseId);
        }

        public List<SelectListItem> PopulateStudentsList(int courseId)
        {
            if (courseId < 0) throw new ArgumentOutOfRangeException(nameof(courseId));

            var allStudents = _studentRepository.GetAll();

            var currentEnrolledStudents = _courseStudentRepository.GetAll()
                .Where(cs => cs.CourseId == courseId).Select(s => s.Student).ToList();

            return allStudents
                .Select(s => new SelectListItem
                {
                    Value = s.StudentId.ToString(),
                    Text = s.StudentFirstName + " " + s.StudentLastName,
                    Selected = currentEnrolledStudents.Contains(s)
                })
                .OrderBy(s => s.Text)
                .ToList();
        }

        public void ChangeCourseStudentsRelationship(List<string> studentIds, List<CourseStudent> currentStudentsInTheCourse, Course course)
        {
            if (studentIds == null) throw new ArgumentNullException(nameof(studentIds));
            if (currentStudentsInTheCourse == null) throw new ArgumentNullException(nameof(currentStudentsInTheCourse));
            if (course == null) throw new ArgumentNullException(nameof(course));

            if (studentIds != null)
            {
                DeleteCourseStudentsRelationship(studentIds, currentStudentsInTheCourse, course.CourseId);
                AddCourseStudentRelationship(studentIds, currentStudentsInTheCourse, course);
            }
        }

        private void DeleteCourseStudentsRelationship(List<string> studentIds, List<CourseStudent> currentStudentsInTheCourse, int courseId)
        {
            if (studentIds == null) throw new ArgumentNullException(nameof(studentIds));
            if (currentStudentsInTheCourse == null) throw new ArgumentNullException(nameof(currentStudentsInTheCourse));
            if (courseId < 0) throw new ArgumentOutOfRangeException(nameof(courseId));

            foreach (var student in currentStudentsInTheCourse)
            {
                if (!studentIds.Contains(student.StudentId.ToString()))
                {
                    _courseStudentRepository.DeleteCourseStudentRelationshipByIds(courseId, student.StudentId);
                }
            }
        }

        private void AddCourseStudentRelationship(List<string> studentIds, List<CourseStudent> currentStudentsInTheCourse, Course course)
        {
            if (studentIds == null) throw new ArgumentNullException(nameof(studentIds));
            if (currentStudentsInTheCourse == null) throw new ArgumentNullException(nameof(currentStudentsInTheCourse));
            if (course == null) throw new ArgumentNullException(nameof(course));

            foreach (var studentId in studentIds)
            {
                if (!currentStudentsInTheCourse.Where(cs => cs.StudentId == Convert.ToInt32(studentId)).Any())
                {
                    var student = _studentRepository.GetById(Convert.ToInt32(studentId));
                    CourseStudent courseStudent = new()
                    {
                        Course = course,
                        CourseId = course.CourseId,
                        Student = student,
                        StudentId = student.StudentId,
                        Mark = -1
                    };
                    _courseStudentRepository.Add(courseStudent);
                }
            }
        }
    }
}
