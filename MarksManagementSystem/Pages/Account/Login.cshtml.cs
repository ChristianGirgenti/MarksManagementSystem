using MarksManagementSystem.Data.Models;
using MarksManagementSystem.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using MarksManagementSystem.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace MarksManagementSystem.Pages
{
    public class LoginModel : PageModel
    {
        private readonly MarksManagementContext _marksManagementContext;
        private readonly IPasswordCreator _passwordCreator;

        [BindProperty]
        public Credential Credential { get; set; } = new();

        [BindProperty]
        public string UserType { get; set; } = string.Empty;

        public string ErrorMessage { get; set; } = string.Empty;

        public List<Claim> Claims { get; set; } = new();

        public LoginModel(MarksManagementContext context, IPasswordCreator passwordCreator)
        {
            _marksManagementContext = context;
            _passwordCreator = passwordCreator;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                if (UserType == null)
                {
                    ErrorMessage = "You need to select what type of user you are to login";
                    return Page();
                }
                ErrorMessage = "Student email and password can not be empty";
                return Page();
            }

            bool isLoginSuccessfull;
            if (UserType == "tutor")
            {
                isLoginSuccessfull = await LogInTutorIsSuccess();
            }
            else
            {
                isLoginSuccessfull = await LogInStudentIsSuccess();
            }
            
            if (!isLoginSuccessfull)
            {
                ErrorMessage = "Invalid email or password";
                return Page();
            }
            return RedirectToPage("/Index");
        }

        private async Task<bool> LogInTutorIsSuccess()
        {

            var tutor = await _marksManagementContext.Tutor.FirstOrDefaultAsync(x => x.TutorEmail == Credential.Email);

            if (tutor == null) return false;
            if (tutor.PasswordSalt == null) throw new ArgumentNullException(nameof(tutor.PasswordSalt));
            var hashedPassword = _passwordCreator.GenerateHashedPassword(tutor.PasswordSalt, Credential.Password);
            if (tutor.TutorPassword == hashedPassword)
            {
                //Login is success so create claims for the user
                BuildClaimsTutor(tutor);

                var claimsIdentity = new ClaimsIdentity(Claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true
                };
                ClearTempData();
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);
                return true;
            }
            return false;
        }


        private async Task<bool> LogInStudentIsSuccess()
        {

            var student = await _marksManagementContext.Student.FirstOrDefaultAsync(x => x.StudentEmail == Credential.Email);

            if (student == null) return false;
            if (student.PasswordSalt == null) throw new ArgumentNullException(nameof(student.PasswordSalt));
            var hashedPassword = _passwordCreator.GenerateHashedPassword(student.PasswordSalt, Credential.Password);
            if (student.StudentPassword == hashedPassword)
            {
                //Login is success so create claims for the user
                BuildClaimsStudent(student);

                var claimsIdentity = new ClaimsIdentity(Claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true
                };
                ClearTempData();
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);
                return true;
            }
            return false;
        }

        public void ClearTempData()
        {
            if (TempData.ContainsKey("SuccessUpdate"))
            {
                TempData.Remove("SuccessUpdate");
            }
        }


        public void BuildClaimsTutor(Tutor tutor)
        {
            Claims = new List<Claim>
                                {
                                    new Claim("TutorId", tutor.TutorId.ToString()),
                                    new Claim("FirstName", tutor.TutorFirstName),
                                    new Claim("LastName", tutor.TutorLastName),
                                    new Claim("DateOfBirth", tutor.TutorDateOfBirth.ToString("ddMMyy")),
                                    new Claim("Email", tutor.TutorEmail),
                                    new Claim("Role", tutor.IsAdmin ? "Admin" : "Tutor"),
                                    new Claim("Password", tutor.TutorPassword),
                                    new Claim("Salt", Convert.ToBase64String(tutor.PasswordSalt))
                                };
        }

        public void BuildClaimsStudent(Student student)
        {
            Claims = new List<Claim>
                                {
                                    new Claim("StudentId", student.StudentId.ToString()),
                                    new Claim("FirstName", student.StudentFirstName),
                                    new Claim("LastName", student.StudentLastName),
                                    new Claim("DateOfBirth", student.StudentDateOfBirth.ToString("ddMMyy")),
                                    new Claim("Email", student.StudentEmail),
                                    new Claim("Role", "Student"),
                                    new Claim("Password", student.StudentPassword),
                                    new Claim("Salt", Convert.ToBase64String(student.PasswordSalt))
                                };
        }
    }
}
