using MarksManagementSystem.Data.Models;
using MarksManagementSystem.Services.Interfaces;
using MarksManagementSystem.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;

namespace MarksManagementSystem.Pages.Courses
{
    [Authorize(Policy = "Admin")]
    public class EditCourseModel : PageModel
    {
        private readonly IEditCourseService _editCourseService;
        private const int SQL_UNIQUE_CONSTRAINT_EX = 2601;
        private const int SQL_UNIQUE_CONSTRAINT_EX2 = 2627;

        [BindProperty]
        public AddEditCourseViewModel EditCourseViewModel { get; set; } = new AddEditCourseViewModel();
        public Course EditedCourse { get; set; } = new Course();
        public List<SelectListItem> PossibleUnitLeaders { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> OtherTutors { get; set; } = new List<SelectListItem>();
        
        [FromQuery(Name = "CourseId")]
        public int CourseToEditId { get; set; }

        public EditCourseModel(IEditCourseService editCourseService)
        {
            _editCourseService = editCourseService;
        }

        public void OnGet()
        {
            var courseToEdit = _editCourseService.GetCourseToEditById(CourseToEditId);
            courseToEdit.CourseTutors = _editCourseService.GetAllTheCourseTutors(CourseToEditId);
            var unitLeaderId = _editCourseService.GetUnitLeaderId(courseToEdit.CourseTutors.ToList(), CourseToEditId);

            EditCourseViewModel = new AddEditCourseViewModel
            {
                CourseName = courseToEdit.CourseName,
                CourseCredits = courseToEdit.CourseCredits,
                UnitLeaderId = unitLeaderId
            };

            PossibleUnitLeaders = _editCourseService.ShowPossibleUnitLeaderInSelectionList(EditCourseViewModel.UnitLeaderId);
            OtherTutors = _editCourseService.PopulateOtherTutors(unitLeaderId, CourseToEditId);
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) return Page();

            try
            {
                if (EditCourseViewModel == null) throw new ArgumentNullException(nameof(EditCourseViewModel));
                EditCourseViewModel = _editCourseService.FormatNewCourseValues(EditCourseViewModel);
                EditedCourse = _editCourseService.EditCourse(CourseToEditId, EditCourseViewModel);
                _editCourseService.ChangeTutorCourseRelationships(EditCourseViewModel, EditedCourse, Request.Form["Tutors"].ToList());
                return RedirectToPage("ViewAllCourses");
            }
            catch (Exception ex)
            {
                if (ex.InnerException is SqlException sqlEx && (sqlEx.Number == SQL_UNIQUE_CONSTRAINT_EX || sqlEx.Number == SQL_UNIQUE_CONSTRAINT_EX2))
                {
                    ModelState.AddModelError("EditCourseViewModel.CourseName", "A course with the same name already exists.");
                }
                else
                {
                    ModelState.AddModelError("EditCourseViewModel.TutorIds", "Something went wrong while editing the course. Please try again.");
                }
                PossibleUnitLeaders = _editCourseService.ShowPossibleUnitLeaderInSelectionList(CourseToEditId);
                OtherTutors = _editCourseService.PopulateOtherTutors(EditCourseViewModel.UnitLeaderId, CourseToEditId);
                return Page();
            }
        }

        public IActionResult OnPostRedirect(int courseId)
        {
            if (courseId <= 0) throw new ArgumentNullException(nameof(courseId));
            return RedirectToPage("CourseStudentManagement", new { courseId });
        }      
    }
}
