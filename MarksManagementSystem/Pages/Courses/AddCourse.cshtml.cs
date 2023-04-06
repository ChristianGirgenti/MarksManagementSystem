using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using MarksManagementSystem.Data.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using MarksManagementSystem.ViewModel;
using Microsoft.AspNetCore.Authorization;
using MarksManagementSystem.Services.Interfaces;

namespace MarksManagementSystem.Pages.Courses
{
    [Authorize(Policy = "Admin")]
    public class AddCourseModel : PageModel
    {
        private readonly IAddCourseService _addCourseService;
        private const int SQL_UNIQUE_CONSTRAINT_EX = 2601;
        private const int SQL_UNIQUE_CONSTRAINT_EX2 = 2627;

        [BindProperty]
        public AddEditCourseViewModel NewCourseViewModel { get; set; } = new ();

        public Course NewCourse = new();
        public List<SelectListItem> OptionsTutors { get; set; } = new List<SelectListItem>();

        public AddCourseModel(IAddCourseService addCourseService)
        {
            _addCourseService = addCourseService ?? throw new ArgumentNullException(nameof(addCourseService));
        }

        
        public void OnGet()
        {
            OptionsTutors = _addCourseService.GetOtherTutorsInSelectionList();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) return Page();
            
            try
            {
                if (NewCourseViewModel == null) throw new ArgumentNullException(nameof(NewCourseViewModel));
                
                NewCourse = _addCourseService.AddCourse(NewCourseViewModel);
                _addCourseService.AddUnitLeaderLinkToCourse(NewCourseViewModel, NewCourse);
                
                return RedirectToPage("ViewAllCourses");
            }
            catch (Exception ex) {
                if (ex.InnerException is SqlException sqlEx && (sqlEx.Number == SQL_UNIQUE_CONSTRAINT_EX || sqlEx.Number == SQL_UNIQUE_CONSTRAINT_EX2))
                {
                    ModelState.AddModelError("NewCourse.CourseName", "A course with the same name already exists.");
                    OptionsTutors = _addCourseService.GetOtherTutorsInSelectionList();
                }
                else
                {
                    ModelState.AddModelError("NewCourse.UnitLeaderId", "Something went wrong while trying to add a new course. Try again.");
                    OptionsTutors = _addCourseService.GetOtherTutorsInSelectionList();
                }
                return Page();
            }  
        }
    }
}
