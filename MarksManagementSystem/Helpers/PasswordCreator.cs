using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace MarksManagementSystem.Helpers
{
    public class PasswordCreator : IPasswordCreator
    {
        public string GenerateHashedPassword(byte[] salt, string password)
        {
            if (salt == null) throw new ArgumentNullException(nameof(salt));
            if (string.IsNullOrEmpty(password)) throw new ArgumentNullException(nameof(password));

            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
               password: password,
               salt: salt,
               prf: KeyDerivationPrf.HMACSHA256,
               iterationCount: 100000,
               numBytesRequested: 256 / 8));
        }
    }
}
