using System.ComponentModel.DataAnnotations;

namespace MarksManagementSystem.Data.Models
{
    public class Student
    {
        public int StudentId { get; set; }

        [MaxLength(255, ErrorMessage = "Student first name can be max 255 characters")]
        [Required(ErrorMessage = "Student first name can not be empty")]
        public string StudentFirstName { get; set; } = string.Empty;

        [MaxLength(255, ErrorMessage = "Student last name can be max 255 characters")]
        [Required(ErrorMessage = "Student last name can not be empty")]
        public string StudentLastName { get; set; } = string.Empty;

        public string StudentEmail { get; set; } = string.Empty;
        public string StudentPassword { get; set; } = string.Empty;
        public byte[]? PasswordSalt { get; set; }

        [Required(ErrorMessage = "The date of birth is required")]
        public DateTime StudentDateOfBirth { get; set; }

        public ICollection<CourseStudent> CourseStudents { get; set; } = new List<CourseStudent>();

        public override string ToString()
        {
            return StudentFirstName + " " + StudentLastName;
        }
    }
}
