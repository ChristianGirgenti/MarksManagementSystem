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
        private readonly ICourseRepository _courseRepository;
        private readonly ITutorRepository _tutorRepository;
        private readonly ICourseTutorRepository _courseTutorRepository;
        private const int SQL_UNIQUE_CONSTRAINT_EX = 2601;
        private const int SQL_UNIQUE_CONSTRAINT_EX2 = 2627;
        public const string SELECTED_TUTOR_IS_UNIT_LEADER_AND_OTHER_TUTOR = "A tutor cannot be both unit leader and other tutor. The selection of unit leader will be prioritised so the tutor will be the new Unit Leader";

        [BindProperty]
        public AddEditCourseViewModel EditCourseViewModel { get; set; } = new AddEditCourseViewModel();
        public Course? EditedCourse { get; set; }
        public List<SelectListItem> OptionsTutors { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> OtherTutors { get; set; } = new List<SelectListItem>();
        
        [FromQuery(Name = "Id")]
        public int Id { get; set; }

        public EditCourseModel(ICourseRepository courseRepository, ITutorRepository tutorRepository, ICourseTutorRepository courseTutorRepository)
        {
            _courseRepository = courseRepository;
            _tutorRepository = tutorRepository;
            _courseTutorRepository = courseTutorRepository;
        }

       
        public void OnGet()
        {
            var courseToEdit = _courseRepository.GetById(Id);
            courseToEdit.CourseTutors = _courseTutorRepository.GetAll().Where(ct => ct.CourseId == Id).ToList();
            
            var unitLeader = courseToEdit.CourseTutors.FirstOrDefault(ct => ct.CourseId == Id && ct.IsUnitLeader);
            int unitLeaderId = unitLeader != null ? unitLeader.TutorId : 0;

            EditCourseViewModel = new AddEditCourseViewModel
            {
                Name = courseToEdit.Name,
                Credits = courseToEdit.Credits,
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
                EditedCourse = EditCourse(Id, EditCourseViewModel);
                ChangeUnitLeader(EditCourseViewModel, EditedCourse);
                ChangeOtherTutors(EditCourseViewModel, EditedCourse);
                return RedirectToPage("ViewAllCourses");
            }
            catch (Exception ex)
            {
                if (ex.InnerException is SqlException sqlEx && (sqlEx.Number == SQL_UNIQUE_CONSTRAINT_EX || sqlEx.Number == SQL_UNIQUE_CONSTRAINT_EX2))
                {
                    ModelState.AddModelError("EditCourseViewModel.Name", "A course with the same name already exists.");
                    
                    if (EditCourseViewModel == null) throw new ArgumentNullException(nameof(EditCourseViewModel));
                    LoadDropdownSelections();
                }

                if (ex.Message == SELECTED_TUTOR_IS_UNIT_LEADER_AND_OTHER_TUTOR)
                {
                    ModelState.AddModelError("EditCourseViewModel.TutorIds", SELECTED_TUTOR_IS_UNIT_LEADER_AND_OTHER_TUTOR);
                    if (EditCourseViewModel == null) throw new ArgumentNullException(nameof(EditCourseViewModel));
                    LoadDropdownSelections();                 
                }
                return Page();
            }
        }

        public void ShowTutorsInSelectionList(int unitLeaderId)
        {
            if (unitLeaderId <= 0) throw new ArgumentNullException(nameof(unitLeaderId));
            
            var unitLeaders = _courseTutorRepository.GetAll()
                .Where(ct => ct.IsUnitLeader)
                .Select(ct => ct.Tutor);

            var nonUnitLeaders = _tutorRepository.GetAll()
                .Where(t => !unitLeaders.Contains(t))
                .Select(t => new SelectListItem { Value = t.Id.ToString(), Text = t.Name + " " + t.LastName })
                .ToList();

            OptionsTutors = nonUnitLeaders;

            var currentUnitLeader = unitLeaders.FirstOrDefault(uL => uL.Id == unitLeaderId);
           
            if (currentUnitLeader != null)
                OptionsTutors.Insert(0, new SelectListItem { Value = currentUnitLeader.Id.ToString(), Text = currentUnitLeader.Name + " " + currentUnitLeader.LastName });
        }

        public List<SelectListItem> PopulateOtherTutors(int unitLeaderId, int courseId)
        {
            if (courseId <= null) throw new ArgumentOutOfRangeException(nameof(courseId));
            if (unitLeaderId <= 0) throw new ArgumentOutOfRangeException(nameof(unitLeaderId));

            var currentOtherTutors = _courseTutorRepository.GetAll()
                .Where(ct => ct.CourseId == courseId && ct.IsUnitLeader == false).Select(t => t.Tutor).ToList();

            List<SelectListItem> otherTutors = _tutorRepository.GetAll()
                .Where(x => x.Id != unitLeaderId)
                .Select(t => new SelectListItem {
                    Value = t.Id.ToString(),
                    Text = t.Name + " " + t.LastName,
                    Selected = currentOtherTutors.Contains(t)
                })
                .ToList();

            return otherTutors;
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

            _courseRepository.Update(courseEdited);
            return courseEdited;
        }

        public void ChangeUnitLeader(AddEditCourseViewModel editCourseViewModel, Course courseEdited)
        {
            if (editCourseViewModel == null) throw new ArgumentNullException(nameof(editCourseViewModel));
            if (courseEdited == null) throw new ArgumentNullException(nameof(courseEdited)); 

            var newUnitLeader = _tutorRepository.GetAll().SingleOrDefault(t => t.Id == editCourseViewModel.UnitLeaderId);
            if (newUnitLeader != null)
            { 
                var currentCourseTutor = _courseTutorRepository.GetAll()
                    .Where(ct => ct.CourseId == courseEdited.Id && ct.IsUnitLeader == true)
                    .FirstOrDefault();
                if (currentCourseTutor != null)
                {
                    currentCourseTutor.Course = courseEdited;
                    currentCourseTutor.Tutor = newUnitLeader;
                    _courseTutorRepository.Update(currentCourseTutor);
                }
            }
        }

        public void ChangeOtherTutors(AddEditCourseViewModel editCourseViewModel, Course course)
        {
            if (editCourseViewModel == null) throw new ArgumentNullException(nameof(editCourseViewModel));
            if (course == null) throw new ArgumentNullException(nameof(course));
            
            editCourseViewModel.TutorIds = Request.Form["Tutors"].ToList();

            if (editCourseViewModel.TutorIds.Contains(editCourseViewModel.UnitLeaderId.ToString()))
                throw new Exception(SELECTED_TUTOR_IS_UNIT_LEADER_AND_OTHER_TUTOR);

            _courseTutorRepository.DeleteAllOtherTutorsInACourse(course.Id);


            foreach (var tutorId in editCourseViewModel.TutorIds)
            {
                var tutor = _tutorRepository.GetById(Convert.ToInt32(tutorId));

                CourseTutor courseTutor = new()
                {
                    CourseId = course.Id,
                    TutorId = tutor.Id,
                    Course = course,
                    Tutor = tutor,
                    IsUnitLeader = false
                };
                _courseTutorRepository.Add(courseTutor);
            }
        }

        public void LoadDropdownSelections()
        {
            ShowTutorsInSelectionList(EditCourseViewModel.UnitLeaderId);
            OtherTutors = PopulateOtherTutors(EditCourseViewModel.UnitLeaderId, Id);
        }
    }
}
