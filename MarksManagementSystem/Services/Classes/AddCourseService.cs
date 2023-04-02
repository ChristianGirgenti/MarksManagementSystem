using MarksManagementSystem.Data.Models;
using MarksManagementSystem.Data.Repositories;
using MarksManagementSystem.Helpers;
using MarksManagementSystem.Services.Interfaces;
using MarksManagementSystem.ViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace MarksManagementSystem.Services.Classes
{
    public class AddCourseService : IAddCourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly ITutorRepository _tutorRepository;
        private readonly ICourseTutorRepository _courseTutorRepository;

        public AddCourseService(ICourseRepository courseRepository, ITutorRepository tutorRepository, ICourseTutorRepository courseTutorRepository)
        {
            _courseRepository = courseRepository;
            _tutorRepository = tutorRepository;
            _courseTutorRepository = courseTutorRepository;
        }

        public List<SelectListItem> GetOtherTutorsInSelectionList()
        {
            var unitLeaders = _courseTutorRepository.GetAll()
                .Where(ct => ct.IsUnitLeader)
                .Select(ct => ct.Tutor);

            var nonUnitLeaders = _tutorRepository.GetAll()
                .Where(t => !unitLeaders.Contains(t))
                .Select(t => new SelectListItem { Value = t.TutorId.ToString(), Text = t.TutorFirstName + " " + t.TutorLastName })
                .ToList();

            nonUnitLeaders.Insert(0, new SelectListItem { Value = "", Text = "Select one unit leader for this course..." });
            return nonUnitLeaders;
        }

        private static void FormatNewCourseValues(Course newCourse)
        {
            if (newCourse == null) throw new ArgumentNullException(nameof(newCourse));
            newCourse.CourseName = StringUtilities.Capitalise(newCourse.CourseName);
        }

        public Course AddCourse(AddEditCourseViewModel newCourseViewModel)
        {
            if (newCourseViewModel == null) throw new ArgumentNullException(nameof(newCourseViewModel));
            var newCourse = new Course
            {
                CourseName = newCourseViewModel.CourseName,
                CourseCredits = newCourseViewModel.CourseCredits
            };
            FormatNewCourseValues(newCourse);
            _courseRepository.Add(newCourse);
            return newCourse;
        }

        public void AddUnitLeaderLinkToCourse(AddEditCourseViewModel newCourseViewModel, Course newCourse)
        {
            if (newCourseViewModel == null) throw new ArgumentNullException(nameof(newCourseViewModel));
            if (newCourse == null) throw new ArgumentNullException(nameof(newCourse));

            var unitLeader = _tutorRepository.GetAll().SingleOrDefault(t => t.TutorId == newCourseViewModel.UnitLeaderId);
            if (unitLeader != null)
            {
                var courseTutor = new CourseTutor { Course = newCourse, Tutor = unitLeader, IsUnitLeader = true };
                _courseTutorRepository.Add(courseTutor);
            }
        }
    }
}
