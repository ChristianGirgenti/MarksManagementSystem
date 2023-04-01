using MarksManagementSystem.Data.Models;
using MarksManagementSystem.Data.Repositories;
using MarksManagementSystem.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MarksManagementSystem.Pages.Courses
{
    [Authorize(Policy = "Tutor")]

    public class ViewCourseModel : PageModel
    {
        private readonly ICourseRepository _courseRepository;
        private readonly ICourseTutorRepository _courseTutorRepository;
        private readonly ICourseStudentRepository _courseStudentRepository;
        private readonly IStudentRepository _studentRepository;


        [FromQuery(Name = "CourseId")]
        public int CourseId { get; set; }
        public Course Course { get; set; } = new Course();
        public Tutor CourseUnitLeader { get; set; } = new Tutor();
        public List<ViewCourseViewModel> AllStudentsEnrolled { get; set; } = new List<ViewCourseViewModel>();
        public AccountClaims AccountClaims;
        public bool IsUserLoggedInTheUnitLeader = false;
       


        public ViewCourseModel(ICourseRepository courseRepository,
            ICourseTutorRepository courseTutorRepository, 
            ICourseStudentRepository courseStudentRepository, 
            IStudentRepository studentRepository)
        {
            _courseRepository = courseRepository;
            _courseStudentRepository = courseStudentRepository;
            _courseTutorRepository = courseTutorRepository;
            _studentRepository = studentRepository;
        }


        public void OnGet()
        {
            Course = _courseRepository.GetById(CourseId);
            ViewData["Title"] = Course.CourseName;

            AllStudentsEnrolled = GetAllStudentEnrolled();

            AccountClaims = new AccountClaims(HttpContext.User.Claims.ToList());
            CourseUnitLeader = _courseTutorRepository.GetUnitLeaderOfCourse(CourseId);
            if (Convert.ToInt32(AccountClaims.AccountId) == CourseUnitLeader.TutorId)
            {
                IsUserLoggedInTheUnitLeader = true;
            }
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) return Page();
            try
            {
                AllStudentsEnrolled = GetAllStudentEnrolled();
                Course = _courseRepository.GetById(CourseId);

                foreach (var student in AllStudentsEnrolled)
                {
                    Student thisStudent = _studentRepository.GetById(Convert.ToInt32(student.StudentId));
                    var courseStudent = _courseStudentRepository.GetByIds(Course.CourseId, thisStudent.StudentId);
                    if (courseStudent != null)
                    {
                        if (string.IsNullOrEmpty(Request.Form["mark" + student.StudentId]))
                            courseStudent.Mark = -1;
                        else
                            courseStudent.Mark = Convert.ToInt32(Request.Form["mark" + student.StudentId]);
                        _courseStudentRepository.Update(courseStudent);

                    }
                }

                TempData["SuccessMessage"] = "Marks have been saved successfully";
                return RedirectToPage(new { courseId = CourseId });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error while saving the marks: "+ex.Message;
                return RedirectToPage(new { courseId = CourseId });
            }
            
        }

        public List<ViewCourseViewModel> GetAllStudentEnrolled()
        {
            return _courseStudentRepository.GetAllByCourseId(CourseId)
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
