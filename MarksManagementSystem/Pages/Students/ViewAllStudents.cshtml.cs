using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MarksManagementSystem.Pages.Students
{
    [Authorize(Policy = "Admin")]
    public class ViewAllStudentsModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
