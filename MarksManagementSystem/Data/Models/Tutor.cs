using Microsoft.AspNetCore.Components;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MarksManagementSystem.Data.Models
{
    public class Tutor
    {
        public int TutorId { get; set; }

        [MaxLength(255, ErrorMessage = "The first name of the tutor can be max 255 characters")]
        [Required(ErrorMessage = "Tutor First TutorFirstName can not be empty")]
        public string TutorFirstName { get; set; } = string.Empty;

        [MaxLength(255, ErrorMessage = "The last name of the tutor can be max 255 characters")]
        [Required(ErrorMessage = "Last name can not be empty")]
        public string TutorLastName { get; set; } = string.Empty;

        [EmailAddress]
        [Required(ErrorMessage = "Insert a valid email")]
        public string TutorEmail { get; set; } = string.Empty;

        [PasswordPropertyText]
        [MaxLength(30, ErrorMessage = "The password must be between 8 and 30 characters")]
        [MinLength(8, ErrorMessage = "The password must be between 8 and 30 characters")]
        [Required(ErrorMessage = "Insert a valid password")]
        public string TutorPassword { get; set; } = string.Empty;

        public bool IsAdmin { get; set; }
        public ICollection<CourseTutor> CourseTutors { get; set; } = new List<CourseTutor>();

        public override string ToString()
        {
            return TutorFirstName + " " + TutorLastName;
        }
    }

   
}
