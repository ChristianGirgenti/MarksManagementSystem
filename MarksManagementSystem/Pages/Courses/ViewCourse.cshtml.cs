using MarksManagementSystem.Data.Models;
using MarksManagementSystem.Data.Repositories;
using MarksManagementSystem.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MarksManagementSystem.Services.Interfaces;

namespace MarksManagementSystem.Pages.Courses
{
    [Authorize(Policy = "Tutor")]

    public class ViewCourseModel : PageModel
    {
        private readonly IViewCourseService _viewCourseService;


        [FromQuery(Name = "CourseId")]
        public int CourseId { get; set; }
        public Course Course { get; set; } = new Course();
        public Tutor CourseUnitLeader { get; set; } = new Tutor();
        public List<ViewCourseViewModel> AllStudentsEnrolled { get; set; } = new List<ViewCourseViewModel>();
        public AccountClaims AccountClaims;
        public bool IsUserLoggedInTheUnitLeader = false;
        public bool IsSaveMarksButtonDisabled = false;
       


        public ViewCourseModel(IViewCourseService viewCourseService)
        {
            if (viewCourseService == null) throw new ArgumentNullException(nameof(viewCourseService));
            _viewCourseService = viewCourseService;
        }


        public void OnGet()
        {
            Course = _viewCourseService.GetCourseById(CourseId);
            ViewData["Title"] = Course.CourseName;
            AllStudentsEnrolled = _viewCourseService.GetAllStudentEnrolled(CourseId);
            AccountClaims = new AccountClaims(HttpContext.User.Claims.ToList());
            CourseUnitLeader = _viewCourseService.GetUnitLeaderOfCourse(CourseId);
            IsSaveMarksButtonDisabled = AllStudentsEnrolled.Count <= 0;
            IsUserLoggedInTheUnitLeader = Convert.ToInt32(AccountClaims.AccountId) == CourseUnitLeader.TutorId;
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) return Page();
            try
            {
                AllStudentsEnrolled = _viewCourseService.GetAllStudentEnrolled(CourseId);
                Course = _viewCourseService.GetCourseById(CourseId);

                foreach (var student in AllStudentsEnrolled)
                {
                    var mark = Request.Form["Mark" + student.StudentId];
                    _viewCourseService.UpdateMarks(Course, mark, Convert.ToInt32(student.StudentId));
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
    }
}
