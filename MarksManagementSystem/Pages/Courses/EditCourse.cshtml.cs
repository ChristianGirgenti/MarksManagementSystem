using MarksManagementSystem.Data.Models;
using MarksManagementSystem.Data.Repositories;
using MarksManagementSystem.Helpers;
using MarksManagementSystem.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;

namespace MarksManagementSystem.Pages.Courses
{
    public class EditCourseModel : PageModel
    {
        private readonly ICourseRepository courseRepository;
        private readonly ITutorRepository tutorRepository;
        private readonly ICourseTutorRepository courseTutorRepository;
        private const int SQL_UNIQUE_CONSTRAINT_EX = 2601;
        private const int SQL_UNIQUE_CONSTRAINT_EX2 = 2627;

        [BindProperty]
        public AddEditCourseViewModel? EditCourseViewModel { get; set; }
        public Course? EditedCourse { get; set; }
        public List<SelectListItem> OptionsTutors { get; set; } = new List<SelectListItem>();
        
        [FromQuery(Name = "Id")]
        public int Id { get; set; }

        public EditCourseModel(ICourseRepository courseRepository, ITutorRepository tutorRepository, ICourseTutorRepository courseTutorRepository)
        {
            this.courseRepository = courseRepository;
            this.tutorRepository = tutorRepository;
            this.courseTutorRepository = courseTutorRepository;
        }

       
        public void OnGet()
        {
            var courseToEdit = courseRepository.GetById(Id);
            courseToEdit.CourseTutors = courseTutorRepository.GetAll().Where(ct => ct.CourseId == Id).ToList();
            
            int unitLeaderId = courseToEdit.CourseTutors.FirstOrDefault(ct => ct.CourseId == Id && ct.IsUnitLeader).TutorId;


            EditCourseViewModel = new AddEditCourseViewModel
            {
                Name = courseToEdit.Name,
                Credits = courseToEdit.Credits,
                UnitLeaderId = unitLeaderId
            };
            ShowTutorsInSelectionList(EditCourseViewModel.UnitLeaderId);
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) return Page();

            try
            {
                if (EditCourseViewModel == null) throw new ArgumentNullException(nameof(EditCourseViewModel));

                FormatNewCourseValues(EditCourseViewModel);
                EditedCourse = EditCourse(Id, EditCourseViewModel);
                ChangeUnitLeader(EditCourseViewModel, EditedCourse);                
                return RedirectToPage("ViewAllCourses");
            }
            catch (Exception ex)
            {
                if (ex.InnerException is SqlException sqlEx && (sqlEx.Number == SQL_UNIQUE_CONSTRAINT_EX || sqlEx.Number == SQL_UNIQUE_CONSTRAINT_EX2))
                {
                    ModelState.AddModelError("NewCourse.Name", "A course with the same name already exists.");
                    
                    if (EditCourseViewModel == null) throw new ArgumentNullException(nameof(EditCourseViewModel));
                    ShowTutorsInSelectionList(EditCourseViewModel.UnitLeaderId);
                }
                return Page();
            }
        }

        public void ShowTutorsInSelectionList(int unitLeaderId)
        {
            if (unitLeaderId <= 0) throw new ArgumentNullException(nameof(unitLeaderId));
            
            var unitLeaders = courseTutorRepository.GetAll()
                .Where(ct => ct.IsUnitLeader)
                .Select(ct => ct.Tutor);

            var nonUnitLeaders = tutorRepository.GetAll()
                .Where(t => !unitLeaders.Contains(t))
                .Select(t => new SelectListItem { Value = t.Id.ToString(), Text = t.Name + " " + t.LastName })
                .ToList();

            OptionsTutors = nonUnitLeaders;
            var currentUnitLeader = unitLeaders.Where(ht => ht.Id == unitLeaderId).FirstOrDefault();
            OptionsTutors.Insert(0, new SelectListItem { Value = currentUnitLeader.Id.ToString(), Text = currentUnitLeader.Name + " " + currentUnitLeader.LastName });
        }
        
        public void FormatNewCourseValues(AddEditCourseViewModel editCourseViewModel)
        {
            if (editCourseViewModel == null) throw new ArgumentNullException(nameof(editCourseViewModel));
            editCourseViewModel.Name = StringUtilities.Capitalise(editCourseViewModel.Name);
        }
        
        public Course EditCourse(int id, AddEditCourseViewModel editCourseViewModel)
        {
            if (id <= 0) throw new ArgumentNullException(nameof(id));
            if (editCourseViewModel == null) throw new ArgumentNullException(nameof(editCourseViewModel));

            var courseEdited = new Course
            {
                Id = id,
                Name = editCourseViewModel.Name,
                Credits = editCourseViewModel.Credits,
            };

            courseRepository.Update(courseEdited);
            return courseEdited;
        }

        public void ChangeUnitLeader(AddEditCourseViewModel editCourseViewModel, Course courseEdited)
        {
            if (editCourseViewModel == null) throw new ArgumentNullException(nameof(editCourseViewModel));
            if (courseEdited == null) throw new ArgumentNullException(nameof(courseEdited)); 

            var newUnitLeader = tutorRepository.GetAll().SingleOrDefault(t => t.Id == editCourseViewModel.UnitLeaderId);
            if (newUnitLeader != null)
            {
                var currentCourseTutor = courseTutorRepository.GetAll()
                    .Where(ct => ct.CourseId == courseEdited.Id && ct.IsUnitLeader == true)
                    .FirstOrDefault();
                if (currentCourseTutor != null)
                {
                    currentCourseTutor.Course = courseEdited;
                    currentCourseTutor.Tutor = newUnitLeader;
                    courseTutorRepository.Update(currentCourseTutor);
                }
            }
        }
    }
}
