using MarksManagementSystem.Data.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace MarksManagementSystem.Services.Interfaces
{
    public interface ILoginService
    {
        public Task<bool> LogInTutorIsSuccess(Credential credential, HttpContext httpContext);
        public Task<bool> LogInStudentIsSuccess(Credential credential, HttpContext context);
        public void BuildClaimsTutor(Tutor tutor);
        public void BuildClaimsStudent(Student student);
    }
}
