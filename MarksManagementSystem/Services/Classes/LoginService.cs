using MarksManagementSystem.Data.Models;
using MarksManagementSystem.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using MarksManagementSystem.Helpers;
using MarksManagementSystem.Services.Interfaces;

namespace MarksManagementSystem.Services.Classes
{
    public class LoginService : ILoginService
    {
        private readonly MarksManagementContext _marksManagementContext;
        private readonly IPasswordCreator _passwordCreator;
        private List<Claim> Claims = new();

        public LoginService(MarksManagementContext context, IPasswordCreator passwordCreator)
        {
            _marksManagementContext = context ?? throw new ArgumentNullException(nameof(context));
            _passwordCreator = passwordCreator ?? throw new ArgumentNullException(nameof(passwordCreator));
        }

        public async Task<bool> LogInTutorIsSuccess(Credential credential, HttpContext httpContext)
        {
            if (credential == null) throw new ArgumentNullException(nameof(credential));
            if (httpContext == null) throw new ArgumentNullException(nameof(httpContext));

            var tutor = await _marksManagementContext.Tutor.FirstOrDefaultAsync(x => x.TutorEmail == credential.Email);

            if (tutor == null) return false;
            if (tutor.PasswordSalt == null) throw new ArgumentNullException(nameof(tutor.PasswordSalt));
            var hashedPassword = _passwordCreator.GenerateHashedPassword(tutor.PasswordSalt, credential.Password);
            if (tutor.TutorPassword == hashedPassword)
            {
                //Login is success so create claims for the user
                BuildClaimsTutor(tutor);

                var claimsIdentity = new ClaimsIdentity(Claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true
                };
                await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);
                return true;
            }
            return false;
        }


        public async Task<bool> LogInStudentIsSuccess(Credential credential, HttpContext httpContext)
        {
            var student = await _marksManagementContext.Student.FirstOrDefaultAsync(x => x.StudentEmail == credential.Email);

            if (student == null) return false;
            if (student.PasswordSalt == null) throw new ArgumentNullException(nameof(student.PasswordSalt));
            var hashedPassword = _passwordCreator.GenerateHashedPassword(student.PasswordSalt, credential.Password);
            if (student.StudentPassword == hashedPassword)
            {
                BuildClaimsStudent(student);

                var claimsIdentity = new ClaimsIdentity(Claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true
                };
                await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);
                return true;
            }
            return false;
        }

        public void BuildClaimsTutor(Tutor tutor)
        {
            if (tutor == null) throw new ArgumentNullException(nameof(tutor));
            Claims = new List<Claim>
                                {
                                    new Claim("AccountId", tutor.TutorId.ToString()),
                                    new Claim("FirstName", tutor.TutorFirstName),
                                    new Claim("LastName", tutor.TutorLastName),
                                    new Claim("DateOfBirth", tutor.TutorDateOfBirth.ToString("ddMMyy")),
                                    new Claim("Email", tutor.TutorEmail),
                                    new Claim("Role", tutor.IsAdmin ? "Admin" : "NoAdmin"),
                                    new Claim("Password", tutor.TutorPassword),
                                    new Claim("Salt", Convert.ToBase64String(tutor.PasswordSalt)),
                                    new Claim("UserType", "Tutor")
                                };
        }

        public void BuildClaimsStudent(Student student)
        {
            if (student == null) throw new ArgumentNullException(nameof(student));
            Claims = new List<Claim>
                                {
                                    new Claim("AccountId", student.StudentId.ToString()),
                                    new Claim("FirstName", student.StudentFirstName),
                                    new Claim("LastName", student.StudentLastName),
                                    new Claim("DateOfBirth", student.StudentDateOfBirth.ToString("ddMMyy")),
                                    new Claim("Email", student.StudentEmail),
                                    new Claim("Role", "NoAdmin"),
                                    new Claim("Password", student.StudentPassword),
                                    new Claim("Salt", Convert.ToBase64String(student.PasswordSalt)),
                                    new Claim("UserType", "Student")
                                };
        }
    }
}
