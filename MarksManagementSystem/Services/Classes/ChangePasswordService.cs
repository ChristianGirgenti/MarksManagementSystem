using MarksManagementSystem.Data.Models;
using MarksManagementSystem.Data.Repositories.Interfaces;
using MarksManagementSystem.Helpers;
using MarksManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace MarksManagementSystem.Services.Classes
{
    public class ChangePasswordService : IChangePasswordService
    {
        private readonly IPasswordCreator _passwordCreator;
        private readonly ITutorRepository _tutorRepository;
        private readonly IStudentRepository _studentRepository;

        public ChangePasswordService(IPasswordCreator passwordCreator, ITutorRepository tutorRepository, IStudentRepository studentRepository)
        {
            _passwordCreator = passwordCreator ?? throw new ArgumentNullException(nameof(passwordCreator));
            _tutorRepository = tutorRepository ?? throw new ArgumentNullException(nameof(tutorRepository));
            _studentRepository = studentRepository ?? throw new ArgumentNullException(nameof(studentRepository));
        }

        public async Task ChangePassword(HttpContext context, string currentPassword, string newPassword)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (string.IsNullOrEmpty(currentPassword)) throw new ArgumentNullException(nameof(currentPassword));
            if (string.IsNullOrEmpty(newPassword)) throw new ArgumentNullException(nameof(newPassword));

            var accountClaims = new AccountClaims(context.User.Claims.ToList());
            var hashedCurrentPasswordForm = _passwordCreator.GenerateHashedPassword(accountClaims.AccountPasswordSalt, currentPassword);
            if (hashedCurrentPasswordForm == accountClaims.AccountPassword)
            {
                var newHashedPassword = _passwordCreator.GenerateHashedPassword(accountClaims.AccountPasswordSalt, newPassword);
                if (accountClaims.AccountUserType.Equals("Tutor"))
                {
                    _tutorRepository.UpdatePasswordByTutorId(Convert.ToInt32(accountClaims.AccountId), newHashedPassword);
                }
                else
                {
                    _studentRepository.UpdatePasswordByStudentId(Convert.ToInt32(accountClaims.AccountId), newHashedPassword);
                }
                await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
            else
            {
                throw new Exception("The current password insterted is wrong.");
            }
        }
    }
}
