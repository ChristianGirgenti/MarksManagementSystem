using System.Security.Claims;

namespace MarksManagementSystem.Data.Models
{
    //Class created to avoid to get information from database everytime
    public class AccountClaims
    {
        public string AccountId { get; set; }
        public string AccountFirstName { get; set; }
        public string AccountLastName { get; set; }
        public string AccountPassword { get; set; }
        public byte[] AccountPasswordSalt { get; set; }
        public string AccountDateOfBirth { get; set; }
        public string AccountUserType { get; set; }

        public AccountClaims(List<Claim> claims)
        {
            if (claims == null) throw new ArgumentNullException(nameof(claims));
            AccountId = claims.FirstOrDefault(c => c.Type == "AccountId")?.Value;
            AccountUserType = claims.FirstOrDefault(c => c.Type == "UserType")?.Value;
            AccountFirstName = claims.FirstOrDefault(c => c.Type == "FirstName")?.Value;
            AccountLastName = claims.FirstOrDefault(c => c.Type == "LastName")?.Value;
            AccountPassword = claims.FirstOrDefault(c => c.Type == "Password")?.Value;
            AccountDateOfBirth = claims.FirstOrDefault(c => c.Type == "DateOfBirth")?.Value;
            var encodedSalt = claims.FirstOrDefault(c => c.Type == "Salt")?.Value;
            AccountPasswordSalt = Convert.FromBase64String(encodedSalt);
        }

    }
}
