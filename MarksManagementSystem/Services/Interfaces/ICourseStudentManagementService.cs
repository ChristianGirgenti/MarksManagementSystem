using MarksManagementSystem.Data.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MarksManagementSystem.Services.Interfaces
{
    public interface ICourseStudentManagementService
    {
        public List<SelectListItem> PopulateStudentsList(int courseId);
        public Course GetCourseById(int courseId);
        public List<CourseStudent> GetAllCurrentStudentsInTheCourse(int courseId);
        public void ChangeCourseStudentsRelationship(List<string> studentIds, List<CourseStudent> currentStudentsInTheCourse, Course course);



    }
}
