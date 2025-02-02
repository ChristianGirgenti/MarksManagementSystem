﻿using MarksManagementSystem.Data.Models;

namespace MarksManagementSystem.Data.Repositories.Interfaces
{
    public interface ICourseStudentRepository
    {
        public void Add(CourseStudent courseStudent);
        public void Update(CourseStudent courseStudent);
        public CourseStudent GetByIds(int courseId, int studentId);
        public List<CourseStudent> GetAll();
        public List<CourseStudent> GetAllByStudentId(int studentId);
        public List<CourseStudent> GetAllByCourseId(int courseId);
        public void DeleteCourseStudentRelationshipByIds(int courseId, int studentId);
        public List<string> GetEnrolledCoursesNameByStudentId(int studentId);

    }
}
