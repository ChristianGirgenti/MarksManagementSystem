using MarksManagementSystem.ViewModel;
using MarksManagementSystem.Services.Interfaces;
using MarksManagementSystem.Data.Repositories.Interfaces;

namespace MarksManagementSystem.Services.Classes
{
    public class ViewAllStudentsService : IViewAllStudentsService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly ICourseStudentRepository _courseStudentRepository;
        public List<ViewAllStudentsViewModel> AllStudentsViewModel { get; set; } = new List<ViewAllStudentsViewModel>();

        public ViewAllStudentsService(IStudentRepository studentRepository, ICourseStudentRepository courseStudentRepository)
        {
            _studentRepository = studentRepository;
            _courseStudentRepository = courseStudentRepository;
        }

        public List<ViewAllStudentsViewModel> GetAllStudentsViewModel()
        {
            return _studentRepository.GetAll()
               .Select(s => new ViewAllStudentsViewModel
               {
                   StudentId = s.StudentId,
                   StudentFullName = s.StudentFirstName + " " + s.StudentLastName,
                   StudentEmail = s.StudentEmail,
                   StudentDateOfBirth = s.StudentDateOfBirth.Date.ToString("d"),
                   StudentEnrolledCourses = string.Join(", ", _courseStudentRepository.GetEnrolledCoursesNameByStudentId(s.StudentId))
               })
               .ToList();
        }

        public void DeleteStudent(int studentId)
        {
            if (studentId < 0) throw new ArgumentOutOfRangeException(nameof(studentId));
            _studentRepository.Delete(studentId);
        }
    }
}
