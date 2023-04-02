using MarksManagementSystem.ViewModel;

namespace MarksManagementSystem.Services.Interfaces
{
    public interface IViewAllStudentsService
    {
        public List<ViewAllStudentsViewModel> GetAllStudentsViewModel();
        public void DeleteStudent(int studentId);
    }
}
