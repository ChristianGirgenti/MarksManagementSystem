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
        private readonly ITeacherRepository teacherRepository;
        private readonly ICourseTeacherRepository courseTeacherRepository;
        private const int SQL_UNIQUE_CONSTRAINT_EX = 2601;
        private const int SQL_UNIQUE_CONSTRAINT_EX2 = 2627;

        [BindProperty]
        public AddEditCourseViewModel? EditCourseViewModel { get; set; }
        public Course? EditedCourse { get; set; }
        public List<SelectListItem> OptionsTeachers { get; set; } = new List<SelectListItem>();
        
        [FromQuery(Name = "Id")]
        public int Id { get; set; }

        public EditCourseModel(ICourseRepository courseRepository, ITeacherRepository teacherRepository, ICourseTeacherRepository courseTeacherRepository)
        {
            this.courseRepository = courseRepository;
            this.teacherRepository = teacherRepository;
            this.courseTeacherRepository = courseTeacherRepository;
        }

       
        public void OnGet()
        {
            var courseToEdit = courseRepository.GetById(Id);
            courseToEdit.CourseTeachers = courseTeacherRepository.GetAll().Where(ct => ct.CourseId == Id).ToList();
            
            int headTeacherId = courseToEdit.CourseTeachers.FirstOrDefault(ct => ct.CourseId == Id && ct.IsHeadTeacher).TeacherId;


            EditCourseViewModel = new AddEditCourseViewModel
            {
                Name = courseToEdit.Name,
                Credits = courseToEdit.Credits,
                HeadTeacherId = headTeacherId
            };
            ShowTeachersInSelectionList(EditCourseViewModel.HeadTeacherId);
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid) return Page();

            try
            {
                if (EditCourseViewModel == null) throw new ArgumentNullException(nameof(EditCourseViewModel));

                FormatNewCourseValues(EditCourseViewModel);
                EditedCourse = EditCourse(Id, EditCourseViewModel);
                EditHeadTeacher(EditCourseViewModel, EditedCourse);                
                return RedirectToPage("ViewAllCourses");
            }
            catch (Exception ex)
            {
                if (ex.InnerException is SqlException sqlEx && (sqlEx.Number == SQL_UNIQUE_CONSTRAINT_EX || sqlEx.Number == SQL_UNIQUE_CONSTRAINT_EX2))
                {
                    ModelState.AddModelError("NewCourse.Name", "A course with the same name already exists.");
                    
                    if (EditCourseViewModel == null) throw new ArgumentNullException(nameof(EditCourseViewModel));
                    ShowTeachersInSelectionList(EditCourseViewModel.HeadTeacherId);
                }
                return Page();
            }
        }

        public void ShowTeachersInSelectionList(int headTeacherId)
        {
            if (headTeacherId <= 0) throw new ArgumentNullException(nameof(headTeacherId));
            
            var headTeachers = courseTeacherRepository.GetAll()
                .Where(ct => ct.IsHeadTeacher)
                .Select(ct => ct.Teacher);

            var nonHeadTeachers = teacherRepository.GetAll()
                .Where(t => !headTeachers.Contains(t))
                .Select(t => new SelectListItem { Value = t.Id.ToString(), Text = t.Name + " " + t.LastName })
                .ToList();

            OptionsTeachers = nonHeadTeachers;
            var currentHeadTeacher = headTeachers.Where(ht => ht.Id == headTeacherId).FirstOrDefault();
            OptionsTeachers.Insert(0, new SelectListItem { Value = currentHeadTeacher.Id.ToString(), Text = currentHeadTeacher.Name + " " + currentHeadTeacher.LastName });
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

        public void EditHeadTeacher(AddEditCourseViewModel editCourseViewModel, Course courseEdited)
        {
            if (editCourseViewModel == null) throw new ArgumentNullException(nameof(editCourseViewModel));
            if (courseEdited == null) throw new ArgumentNullException(nameof(courseEdited)); 

            var newHeadTeacher = teacherRepository.GetAll().SingleOrDefault(t => t.Id == editCourseViewModel.HeadTeacherId);
            if (newHeadTeacher != null)
            {
                var currentCourseTeacher = courseTeacherRepository.GetAll()
                    .Where(ct => ct.CourseId == courseEdited.Id && ct.IsHeadTeacher == true)
                    .FirstOrDefault();
                if (currentCourseTeacher != null)
                {
                    currentCourseTeacher.Course = courseEdited;
                    currentCourseTeacher.Teacher = newHeadTeacher;
                    courseTeacherRepository.Update(currentCourseTeacher);
                }
            }
        }
    }
}
