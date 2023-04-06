using MarksManagementSystem.ViewModel;

namespace MarksManagementSystem.Services.Interfaces
{
    public interface IViewAllCoursesService
    {
        public List<ViewAllCoursesViewModel> GetAllCoursesWithTutors();
        public void DeleteCourse(int courseId);


    }
}
