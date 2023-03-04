﻿using MarksManagementSystem.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace MarksManagementSystem.Data.Repositories
{
    public class CourseTeacherRepository : ICourseTeacherRepository
    {
        private MarksManagementContext marksManagementContext;

        public CourseTeacherRepository(MarksManagementContext context)
        {
            marksManagementContext = context;
        }
        public void Add(CourseTeacher courseTeacher)
        {
            marksManagementContext.CourseTeachers.Add(courseTeacher);
            marksManagementContext.SaveChanges();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<CourseTeacher> GetAll()
        {
            return marksManagementContext.CourseTeachers
                .Include(ct => ct.Teacher)
                .Include(ct => ct.Course)
                .ToList();  
        }

        public void GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void Update(CourseTeacher course)
        {
            throw new NotImplementedException();
        }
    }
}
