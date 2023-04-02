using MarksManagementSystem.Data.Models;
using MarksManagementSystem.Data.Repositories;
using MarksManagementSystem.Helpers;
using MarksManagementSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace MarksManagementSystem.Services.Classes
{
    public class ChangePasswordService : IChangePasswordService
    {
        private readonly IPasswordCreator _passwordCreator;
        private readonly ITutorRepository _tutorRepository;
        private readonly IStudentRepository _studentRepository;

        public ChangePasswordService(IPasswordCreator passwordCreator, ITutorRepository tutorRepository, IStudentRepository studentRepository)
        {
            _passwordCreator = passwordCreator;
            _tutorRepository = tutorRepository;
            _studentRepository = studentRepository;
        }

        public async Task ChangePassword(HttpContext context, string currentPassword, string newPassword)
        {
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
