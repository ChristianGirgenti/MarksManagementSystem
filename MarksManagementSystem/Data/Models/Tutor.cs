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

        public string TutorEmail { get; set; } = string.Empty;

        public string TutorPassword { get; set; } = string.Empty;

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
