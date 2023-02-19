using System.ComponentModel.DataAnnotations;

namespace MarksManagementSystem.Data
{
    public class Teacher
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string LastName { get; set; }
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
