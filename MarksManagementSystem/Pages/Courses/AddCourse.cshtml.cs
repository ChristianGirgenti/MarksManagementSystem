using Microsoft.AspNetCore.Mvc.RazorPages;
using MarksManagementSystem.Data;
using Microsoft.AspNetCore.Mvc;

namespace MarksManagementSystem.Pages.Courses
{
    public class AddCourseModel : PageModel
    {
        [BindProperty]
        public Course NewCourse { get; set; }

        public void OnGet()
        {

        }

        public IActionResult OnPost()
        {
            // SAVE PRODUCT TO DATABASE
            if (ModelState.IsValid)
            {
                var courseName = NewCourse.Name;
                return RedirectToPage("ViewAllCourses");
            }
            return Page();
        }
    }
}
