using System.ComponentModel.DataAnnotations;

namespace MarksManagementSystem.Data
{
    public class Course
    {
        public int Id { get; set; }

        [MaxLength(50, ErrorMessage = "The name of the course can be max 50 characters")]
        [Required]
        public string Name { get; set; }
    }
}
