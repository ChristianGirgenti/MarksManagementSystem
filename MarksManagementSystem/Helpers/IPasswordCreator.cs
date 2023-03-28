namespace MarksManagementSystem.Helpers
{
    public interface IPasswordCreator
    {
        public string GenerateHashedPassword(byte[] salt, string password);
    }
}
