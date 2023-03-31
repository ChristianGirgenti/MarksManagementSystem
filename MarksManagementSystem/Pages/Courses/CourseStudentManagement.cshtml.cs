using MarksManagementSystem.Data.Models;
using MarksManagementSystem.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MarksManagementSystem.Pages.Courses
{
    public class CourseStudentManagementModel : PageModel
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly ICourseStudentRepository _courseStudentRepository;

        [FromQuery(Name = "CourseId")]
        public int CourseId { get; set; }
        public Course ThisCourse { get; set; } = new Course();
        public List<SelectListItem> AllStudents { get; set; } = new List<SelectListItem>();
        public List<CourseStudent> CurrentStudentsInTheCourse { get; set; } = new();



        public CourseStudentManagementModel(ICourseRepository courseRepository, IStudentRepository studentRepository, ICourseStudentRepository courseStudentRepository)
        {
            _courseRepository = courseRepository;
            _studentRepository = studentRepository;
            _courseStudentRepository = courseStudentRepository;
        }


        public void OnGet()
        {
            ThisCourse = _courseRepository.GetById(CourseId);
            ViewData["Title"] = "Manage Students For " + ThisCourse.CourseName;

            PopulateStudentsList();
        }

        public IActionResult OnPost()
        {
            ThisCourse = _courseRepository.GetById(CourseId);
            CurrentStudentsInTheCourse = _courseStudentRepository.GetAllByCourseId(CourseId);
            try
            {
                ChangeCourseStudentsRelationship();
                TempData["SuccessMessage"] = "The students and course relationship have been update successfully.";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while changing course and students relationship: " + ex.Message;
            }
            PopulateStudentsList();
            return RedirectToPage("ViewAllCourses");
        }

        public void PopulateStudentsList()
        {
            var allStudents = _studentRepository.GetAll();

            var currentEnrolledStudents = _courseStudentRepository.GetAll()
                .Where(cs => cs.CourseId == CourseId).Select(s => s.Student).ToList();

            AllStudents = allStudents
                .Select(s => new SelectListItem
                {
                    Value = s.StudentId.ToString(),
                    Text = s.StudentFirstName + " " + s.StudentLastName,
                    Selected = currentEnrolledStudents.Contains(s)
                })
                .OrderBy(s => s.Text)
                .ToList();
        }

        public void ChangeCourseStudentsRelationship()
        {
            var studentIds = Request.Form["Students"].ToList();
            if (studentIds != null) {
                DeleteCourseStudentsRelationship(studentIds);
                AddCourseStudentRelationship(studentIds);
            }
        }

        public void DeleteCourseStudentsRelationship(List<string> studentIds)
        {
            foreach (var student in CurrentStudentsInTheCourse)
            {
                if (!studentIds.Contains(student.StudentId.ToString()))
                {
                    _courseStudentRepository.DeleteCourseStudentRelationshipByIds(CourseId, student.StudentId);
                }
            }
        }

        public void AddCourseStudentRelationship(List<string> studentIds)
        {
            foreach (var studentId in studentIds)
            {
                if (!CurrentStudentsInTheCourse.Where(cs => cs.StudentId == Convert.ToInt32(studentId)).Any())
                {
                    var student = _studentRepository.GetById(Convert.ToInt32(studentId));
                    CourseStudent courseStudent = new()
                    {
                        Course = ThisCourse,
                        CourseId = CourseId,
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
