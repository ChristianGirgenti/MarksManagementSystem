namespace MarksManagementSystem.Services.Interfaces
{
    public interface IChangePasswordService
    {
        public Task ChangePassword(HttpContext context, string currentPassword, string newPassword);
    }
}
