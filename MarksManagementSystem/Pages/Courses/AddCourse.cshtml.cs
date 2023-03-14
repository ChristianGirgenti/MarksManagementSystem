using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using MarksManagementSystem.Data.Models;
using MarksManagementSystem.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using MarksManagementSystem.Helpers;
using MarksManagementSystem.ViewModel;
using MarksManagementSystem.Data.Repositories;

namespace MarksManagementSystem.Pages.Courses
{
    public class AddCourseModel : PageModel
    {
        private readonly ICourseRepository courseRepository;
        private readonly ITutorRepository tutorRepository;
        private readonly ICourseTutorRepository courseTutorRepository;
        private const int SQL_UNIQUE_CONSTRAINT_EX = 2601;
        private const int SQL_UNIQUE_CONSTRAINT_EX2 = 2627;


        public AddCourseModel(ICourseRepository courseRepository, ITutorRepository tutorRepository, ICourseTutorRepository courseTutorRepository)
        {
            this.courseRepository = courseRepository;
            this.tutorRepository = tutorRepository;
            this.courseTutorRepository = courseTutorRepository; 
        }

        [BindProperty]
        public AddEditCourseViewModel? NewCourseViewModel { get; set; }
        
        public Course? NewCourse;
        public List<SelectListItem> OptionsTutors { get; set; } = new List<SelectListItem>();
        public void OnGet()
        {
            ShowTutorsInSelectionList();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) return Page();
            
            try
            {
                if (NewCourseViewModel == null) throw new ArgumentNullException(nameof(NewCourseViewModel));
                
                NewCourse = AddCourse(NewCourseViewModel);
                AddUnitLeaderLinkToCourse(NewCourseViewModel, NewCourse);
                
                return RedirectToPage("ViewAllCourses");
            }
            catch (Exception ex) {
                if (ex.InnerException is SqlException sqlEx && (sqlEx.Number == SQL_UNIQUE_CONSTRAINT_EX || sqlEx.Number == SQL_UNIQUE_CONSTRAINT_EX2))
                {
                    ModelState.AddModelError("NewCourse.Name", "A course with the same name already exists.");
                    ShowTutorsInSelectionList();
                }
                return Page();
            }  
        }

        public void ShowTutorsInSelectionList()
        {

            var unitLeaders = courseTutorRepository.GetAll()
                .Where(ct => ct.IsUnitLeader)
                .Select(ct => ct.Tutor);

            var nonUnitLeaders = tutorRepository.GetAll()
                .Where(t => !unitLeaders.Contains(t))
                .Select(t => new SelectListItem { Value = t.Id.ToString(), Text = t.Name + " " + t.LastName })
                .ToList();

            OptionsTutors = nonUnitLeaders;
            OptionsTutors.Insert(0, new SelectListItem { Value = "", Text = "Select one unit leader for this course..." });
        }
        public void FormatNewCourseValues(Course newCourse)
        {
            if (newCourse == null) throw new ArgumentNullException(nameof(newCourse));
            newCourse.Name = StringUtilities.Capitalise(newCourse.Name);
        }
        public Course AddCourse(AddEditCourseViewModel newCourseViewModel)
        {
            if (newCourseViewModel == null) throw new ArgumentNullException(nameof(newCourseViewModel));
            NewCourse = new Course
            {
                Name = newCourseViewModel.Name,
                Credits = newCourseViewModel.Credits
            };
            FormatNewCourseValues(NewCourse);
            courseRepository.Add(NewCourse);
            return NewCourse;
        }
        public void AddUnitLeaderLinkToCourse(AddEditCourseViewModel newCourseViewModel, Course newCourse)
        {
            if (newCourseViewModel == null) throw new ArgumentNullException(nameof(newCourseViewModel));
            if (newCourse == null) throw new ArgumentNullException(nameof(newCourse));

            var unitLeader = tutorRepository.GetAll().SingleOrDefault(t => t.Id == newCourseViewModel.UnitLeaderId);
            if (unitLeader != null)
            {
                var courseTutor = new CourseTutor { Course = newCourse, Tutor = unitLeader, IsUnitLeader = true };
                courseTutorRepository.Add(courseTutor);
            }
        }
    }
}
