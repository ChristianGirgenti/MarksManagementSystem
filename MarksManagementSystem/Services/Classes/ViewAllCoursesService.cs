using MarksManagementSystem.Data.Repositories.Interfaces;
using MarksManagementSystem.Services.Interfaces;
using MarksManagementSystem.ViewModel;

namespace MarksManagementSystem.Services.Classes
{
    public class ViewAllCoursesService : IViewAllCoursesService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly ICourseTutorRepository _courseTutorRepository;

        public ViewAllCoursesService(ICourseRepository courseRepository, ICourseTutorRepository courseTutorRepository)
        {
            _courseRepository = courseRepository ?? throw new ArgumentNullException(nameof(courseRepository)); ;
            _courseTutorRepository = courseTutorRepository ?? throw new ArgumentNullException(nameof(courseTutorRepository)); ;
        }

        public List<ViewAllCoursesViewModel> GetAllCoursesWithTutors()
        {
            return _courseRepository.GetAll()
                .Select(c => new ViewAllCoursesViewModel
                {
                    CourseId = c.CourseId,
                    CourseName = c.CourseName,
                    CourseCredits = c.CourseCredits,
                    UnitLeader = _courseTutorRepository.GetUnitLeaderOfCourse(c.CourseId).ToString(),
                    OtherTutors = string.Join(", ", _courseTutorRepository.GetOtherTutorsOfCourseToString(c.CourseId))
                })
                .ToList();
        }

        public void DeleteCourse(int courseId)
        {
            if (courseId <= 0) throw new ArgumentOutOfRangeException(nameof(courseId));
            _courseRepository.Delete(courseId);
        }
    }
}
