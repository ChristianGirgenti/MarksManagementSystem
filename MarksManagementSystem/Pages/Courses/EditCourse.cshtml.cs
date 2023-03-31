using MarksManagementSystem.Data.Models;
using MarksManagementSystem.Data.Repositories;
using MarksManagementSystem.Helpers;
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
        private readonly ICourseRepository _courseRepository;
        private readonly ITutorRepository _tutorRepository;
        private readonly ICourseTutorRepository _courseTutorRepository;
        private const int SQL_UNIQUE_CONSTRAINT_EX = 2601;
        private const int SQL_UNIQUE_CONSTRAINT_EX2 = 2627;

        [BindProperty]
        public AddEditCourseViewModel EditCourseViewModel { get; set; } = new AddEditCourseViewModel();
        public Course EditedCourse { get; set; } = new Course();
        public List<SelectListItem> OptionsTutors { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> OtherTutors { get; set; } = new List<SelectListItem>();
        
        [FromQuery(Name = "CourseId")]
        public int CourseToEditId { get; set; }

        public EditCourseModel(ICourseRepository courseRepository, ITutorRepository tutorRepository, ICourseTutorRepository courseTutorRepository)
        {
            _courseRepository = courseRepository;
            _tutorRepository = tutorRepository;
            _courseTutorRepository = courseTutorRepository;
        }

       
        public void OnGet()
        {
            var courseToEdit = _courseRepository.GetById(CourseToEditId);
            courseToEdit.CourseTutors = _courseTutorRepository.GetAll().Where(ct => ct.CourseId == CourseToEditId).ToList();
            
            var unitLeader = courseToEdit.CourseTutors.FirstOrDefault(ct => ct.CourseId == CourseToEditId && ct.IsUnitLeader);
            int unitLeaderId = unitLeader != null ? unitLeader.TutorId : 0;

            EditCourseViewModel = new AddEditCourseViewModel
            {
                CourseName = courseToEdit.CourseName,
                CourseCredits = courseToEdit.CourseCredits,
                UnitLeaderId = unitLeaderId
            };
            LoadDropdownSelections();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) return Page();

            try
            {
                if (EditCourseViewModel == null) throw new ArgumentNullException(nameof(EditCourseViewModel));
                FormatNewCourseValues(EditCourseViewModel);
                EditedCourse = EditCourse(CourseToEditId, EditCourseViewModel);
                ChangeTutorCourseRelationships(EditCourseViewModel, EditedCourse);
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
                LoadDropdownSelections();
                return Page();
            }
        }

        public IActionResult OnPostRedirect(int courseId)
        {
            if (courseId <= 0) throw new ArgumentNullException(nameof(courseId));
            return RedirectToPage("CourseStudentManagement", new { courseId });
        }

        public void ShowTutorsInSelectionList(int unitLeaderId)
        {
            if (unitLeaderId <= 0) throw new ArgumentNullException(nameof(unitLeaderId));

            IEnumerable<Tutor> unitLeaders = _courseTutorRepository.GetAll()
                .Where(ct => ct.IsUnitLeader)
                .Select(ct => ct.Tutor);

            List<SelectListItem> nonUnitLeaders = _tutorRepository.GetAll()
                .Where(t => !unitLeaders.Contains(t))
                .Select(t => new SelectListItem { Value = t.TutorId.ToString(), Text = t.TutorFirstName + " " + t.TutorLastName })
                .ToList();

            OptionsTutors = nonUnitLeaders;

            var currentUnitLeader = unitLeaders.FirstOrDefault(uL => uL.TutorId == unitLeaderId);
           
            if (currentUnitLeader != null)
                OptionsTutors.Insert(0, new SelectListItem { Value = currentUnitLeader.TutorId.ToString(), Text = currentUnitLeader.TutorFirstName + " " + currentUnitLeader.TutorLastName });
        }

        public List<SelectListItem> PopulateOtherTutors(int unitLeaderId, int courseId)
        {
            if (courseId <= 0) throw new ArgumentOutOfRangeException(nameof(courseId));
            if (unitLeaderId <= 0) throw new ArgumentOutOfRangeException(nameof(unitLeaderId));

            var currentOtherTutors = _courseTutorRepository.GetAll()
                .Where(ct => ct.CourseId == courseId && ct.IsUnitLeader == false).Select(t => t.Tutor).ToList();

            List<SelectListItem> otherTutors = _tutorRepository.GetAll()
                .Where(x => x.TutorId != unitLeaderId)
                .Select(t => new SelectListItem {
                    Value = t.TutorId.ToString(),
                    Text = t.TutorFirstName + " " + t.TutorLastName,
                    Selected = currentOtherTutors.Contains(t)
                })
                .ToList();

            return otherTutors;
        }

        public void FormatNewCourseValues(AddEditCourseViewModel editCourseViewModel)
        {
            if (editCourseViewModel == null) throw new ArgumentNullException(nameof(editCourseViewModel));
            editCourseViewModel.CourseName = StringUtilities.Capitalise(editCourseViewModel.CourseName);
        }
        
        public Course EditCourse(int courseId, AddEditCourseViewModel editCourseViewModel)
        {
            if (courseId <= 0) throw new ArgumentNullException(nameof(courseId));
            if (editCourseViewModel == null) throw new ArgumentNullException(nameof(editCourseViewModel));

            var courseEdited = new Course
            {
                CourseId = courseId,
                CourseName = editCourseViewModel.CourseName,
                CourseCredits = editCourseViewModel.CourseCredits,
            };


            _courseRepository.Update(courseEdited);
            return courseEdited;
        }

        public void ChangeTutorCourseRelationships(AddEditCourseViewModel editCourseViewModel, Course courseEdited)
        {
            if (editCourseViewModel == null) throw new ArgumentNullException(nameof(editCourseViewModel));
            if (courseEdited == null) throw new ArgumentNullException(nameof(courseEdited));
           
            _courseTutorRepository.DeleteCourseUnitLeaderRelationshipByCourseId(courseEdited.CourseId);
            _courseTutorRepository.DeleteAllOtherTutorsInACourse(courseEdited.CourseId);

            var newUnitLeader = _tutorRepository.GetAll().SingleOrDefault(t => t.TutorId == editCourseViewModel.UnitLeaderId);
            if (newUnitLeader != null)
                AddUnitLeaderRelationship(newUnitLeader, courseEdited); 

            editCourseViewModel.TutorIds = Request.Form["Tutors"].ToList();

            AddOtherTutorsRelationship(editCourseViewModel.TutorIds, courseEdited);
        }

        public void AddOtherTutorsRelationship(List<string> otherTutors, Course courseEdited)
        {
            if (otherTutors == null) throw new ArgumentNullException(nameof(otherTutors));
            if (courseEdited == null) throw new ArgumentNullException(nameof(courseEdited));

            //If a tutor is selected to be both unitLeader and otherTutor, unit leader will have priority, ignoring the other tutor selection
            foreach (var tutorId in otherTutors)
            {
                var tutor = _tutorRepository.GetById(Convert.ToInt32(tutorId));
                if (tutor.TutorId != EditCourseViewModel.UnitLeaderId)
                {
                    CourseTutor courseTutor = new()
                    {
                        CourseId = courseEdited.CourseId,
                        TutorId = tutor.TutorId,
                        Course = courseEdited,
                        Tutor = tutor,
                        IsUnitLeader = false
                    };
                    _courseTutorRepository.Add(courseTutor);
                }
            }
        }

        public void AddUnitLeaderRelationship(Tutor newUnitLeader, Course courseEdited)
        {
            if (newUnitLeader == null) throw new ArgumentNullException(nameof(newUnitLeader));
            if (courseEdited == null) throw new ArgumentNullException(nameof(courseEdited));

            if (newUnitLeader != null)
            {
                var newCourseTutorRelationship = new CourseTutor()
                {
                    CourseId = courseEdited.CourseId,
                    TutorId = newUnitLeader.TutorId,
                    Course = courseEdited,
                    Tutor = newUnitLeader,
                    IsUnitLeader = true
                };
                _courseTutorRepository.Add(newCourseTutorRelationship);
            }
        }

        public void LoadDropdownSelections()
        {
            ShowTutorsInSelectionList(EditCourseViewModel.UnitLeaderId);
            OtherTutors = PopulateOtherTutors(EditCourseViewModel.UnitLeaderId, CourseToEditId);
        }
    }
}
