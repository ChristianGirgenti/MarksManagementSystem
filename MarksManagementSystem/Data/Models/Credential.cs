using System.ComponentModel.DataAnnotations;

namespace MarksManagementSystem.Data.Models
{
    public class Credential
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }
        
    }
}
