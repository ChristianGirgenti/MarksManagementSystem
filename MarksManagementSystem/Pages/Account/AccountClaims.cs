using MarksManagementSystem.Data.Models;
using System.Security.Claims;

namespace MarksManagementSystem.Pages.Account
{
    //Class created to avoid to get information from database everytime
    public class AccountClaims
    {
        public string AccountId { get; set; }
        public string AccountFirstName { get; set; }
        public string AccountLastName { get; set; }
        public string AccountEmail { get; set; }
        public string AccountPassword { get; set; }
        public byte[] AccountPasswordSalt { get; set; }
        public string AccountDateOfBirth { get; set; }
        public bool AccountIsTutor { get; set; }

        public AccountClaims(List<Claim> claims)
        {
            if (claims == null) throw new ArgumentNullException(nameof(claims));
            if (claims.FirstOrDefault(c => c.Type == "TutorId")?.Value != null)
            {
                AccountIsTutor = true;
                AccountId = claims.FirstOrDefault(c => c.Type == "TutorId")?.Value;
            }
            else if (claims.FirstOrDefault(c => c.Type == "StudentId")?.Value != null)
            {
                AccountIsTutor = false;
                AccountId = claims.FirstOrDefault(c => c.Type == "StudentId")?.Value;
            }
            AccountFirstName = claims.FirstOrDefault(c => c.Type == "FirstName")?.Value;
            AccountLastName = claims.FirstOrDefault(c => c.Type == "LastName")?.Value;
            AccountPassword = claims.FirstOrDefault(c => c.Type == "Password")?.Value;
            AccountDateOfBirth = claims.FirstOrDefault(c => c.Type == "DateOfBirth")?.Value;

            var encodedSalt = claims.FirstOrDefault(c => c.Type == "Salt")?.Value;
            AccountPasswordSalt = Convert.FromBase64String(encodedSalt);
        }

    }
}
