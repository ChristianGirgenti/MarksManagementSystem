using System.ComponentModel.DataAnnotations;

namespace MarksManagementSystem.Data.Models
{
    public class Student
    {
        public int StudentId { get; set; }

        [Required]
        public string StudentFirstName { get; set; } = string.Empty;
        [Required]
        public string StudentLastName { get; set; } = string.Empty;
        [EmailAddress]
        [Required]
        public string StudentEmail { get; set; } = string.Empty;
        [Required]
        public string StudentPassword { get; set; } = string.Empty;
    }
}
