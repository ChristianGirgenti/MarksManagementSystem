using MarksManagementSystem.Data.Models;
using MarksManagementSystem.ViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MarksManagementSystem.Services.Interfaces
{
    public interface IAddCourseService
    {
        public List<SelectListItem> GetOtherTutorsInSelectionList();
        public Course AddCourse(AddEditCourseViewModel newCourseViewModel);
        public void AddUnitLeaderLinkToCourse(AddEditCourseViewModel newCourseViewModel, Course newCourse);

    }
}
