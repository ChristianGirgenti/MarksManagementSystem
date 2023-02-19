using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MarksManagementSystem.Pages
{
    public class LoginModel : PageModel
    {
        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            string email = Request.Form["email"];
            string password = Request.Form["password"];

            Console.WriteLine(email+ " " + password);

            bool isLoginSuccessfull = true;
            bool isTeacher = true;

            //If is teacher and admin redirect to admin view
            //Else is teacher and not admin redirect to teacher view
            //Else rredirect to student view

            if (isLoginSuccessfull && isTeacher)
                return RedirectToPage("/Teachers/AddTeacher");
            else return RedirectToPage("Students/AddStudent");                
        }
    }
}
