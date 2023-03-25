using Microsoft.AspNetCore.Components;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MarksManagementSystem.Data.Models
{
    public class Tutor
    {
        public int TutorId { get; set; }

        [MaxLength(255, ErrorMessage = "Tutor first name can be max 255 characters")]
        [Required(ErrorMessage = "Tutor first name can not be empty")]
        public string TutorFirstName { get; set; } = string.Empty;

        [MaxLength(255, ErrorMessage = "Tutor last name can be max 255 characters")]
        [Required(ErrorMessage = "Tutor last name can not be empty")]
        public string TutorLastName { get; set; } = string.Empty;

        public string TutorEmail { get; set; } = string.Empty;

        public string TutorPassword { get; set; } = string.Empty;

        public byte[]? PasswordSalt { get; set; }

        [Required(ErrorMessage = "The date of birth is required")]
        public DateTime TutorDateOfBirth { get; set; }

        public bool IsAdmin { get; set; }
        public ICollection<CourseTutor> CourseTutors { get; set; } = new List<CourseTutor>();

        public override string ToString()
        {
            return TutorFirstName + " " + TutorLastName;
        }
    }

   
}
