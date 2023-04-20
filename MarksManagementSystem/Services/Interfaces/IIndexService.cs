using MarksManagementSystem.Data.Models;
using MarksManagementSystem.ViewModel;

namespace MarksManagementSystem.Services.Interfaces
{
    public interface IIndexService
    {
        List<ViewAllCoursesViewModel> GetTutorCourses(AccountClaims accountClaims);
        List<StudentIndexViewModel> GetStudentCourses(AccountClaims accountClaims);
        string ConstructDefaultInitialPassword(AccountClaims accountClaims);
    }
}
