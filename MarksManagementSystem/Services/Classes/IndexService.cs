using MarksManagementSystem.Data.Models;
using MarksManagementSystem.Data.Repositories.Interfaces;
using MarksManagementSystem.Helpers;
using MarksManagementSystem.Services.Interfaces;
using MarksManagementSystem.ViewModel;

namespace MarksManagementSystem.Services.Classes
{
    public class IndexService : IIndexService
    {
        private readonly ICourseTutorRepository _courseTutorRepository;
        private readonly ICourseStudentRepository _courseStudentRepository;
        private readonly IPasswordCreator _passwordCreator;

        public IndexService(ICourseTutorRepository courseTutorRepository, ICourseStudentRepository courseStudentRepository, IPasswordCreator passwordCreator)
        {
            _courseTutorRepository = courseTutorRepository ?? throw new ArgumentNullException(nameof(courseTutorRepository));
            _courseStudentRepository = courseStudentRepository ?? throw new ArgumentNullException(nameof(courseStudentRepository));
            _passwordCreator = passwordCreator ?? throw new ArgumentNullException(nameof(passwordCreator));
        }

        public List<ViewAllCoursesViewModel> GetTutorCourses(AccountClaims accountClaims)
        {
            if (accountClaims == null) throw new ArgumentNullException(nameof(accountClaims));

            return _courseTutorRepository.GetAllByTutorId(Convert.ToInt32(accountClaims.AccountId))
            .Select(c => new ViewAllCoursesViewModel
            {
                CourseId = c.CourseId,
                CourseName = c.Course.CourseName,
                CourseCredits = c.Course.CourseCredits,

                UnitLeader = _courseTutorRepository.GetAll()
                    .Where(ct => ct.CourseId == c.CourseId && ct.IsUnitLeader == true)
                    .Select(ct => ct.Tutor.ToString())
                    .SingleOrDefault(),

                OtherTutors = string.Join(", ", _courseTutorRepository.GetAll()
                    .Where(ct => ct.CourseId == c.CourseId && ct.IsUnitLeader == false)
                    .Select(ct => ct.Tutor.ToString())
                    .ToList())
            })
            .ToList();
        }

        public List<StudentIndexViewModel> GetStudentCourses(AccountClaims accountClaims)
        {
            if (accountClaims == null) throw new ArgumentNullException(nameof(accountClaims));

            return _courseStudentRepository.GetAllByStudentId(Convert.ToInt32(accountClaims.AccountId))
            .Select(c => new StudentIndexViewModel
            {
                CourseName = c.Course.CourseName,
                CourseCredits = c.Course.CourseCredits.ToString(),
                Mark = c.Mark.ToString(),
                //The ShowMark field checks that every student in that course have a mark assigned. If any of them still hasn't got a mark
                //Keep the marks not visible to all the students.
                ShowMark = !(_courseStudentRepository.GetAllByCourseId(c.Course.CourseId).Any(m => m.Mark == -1))
            })
            .ToList();
        }

        public string ConstructDefaultInitialPassword(AccountClaims accountClaims)
        {
            if (accountClaims == null) throw new ArgumentNullException(nameof(accountClaims));

            var lastNameLower = accountClaims.AccountLastName.ToLower();
            var startPassword = string.Concat(accountClaims.AccountFirstName.AsSpan(0, 1),
                                              lastNameLower.AsSpan(0, 1),
                                              accountClaims.AccountDateOfBirth,
                                              ".");

            return _passwordCreator.GenerateHashedPassword(accountClaims.AccountPasswordSalt, startPassword);
        }
    }
}
