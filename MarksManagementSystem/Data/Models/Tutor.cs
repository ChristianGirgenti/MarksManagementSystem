using Microsoft.AspNetCore.Components;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MarksManagementSystem.Data.Models
{
    public class Tutor
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name can not be empty")]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "Last name can not be empty")]
        public string LastName { get; set; } = string.Empty;
        [EmailAddress]
        [Required(ErrorMessage = "Insert a valid email")]
        public string Email { get; set; } = string.Empty;
        [PasswordPropertyText]
        [Required(ErrorMessage = "Insert a valid password")]
        public string Password { get; set; } = string.Empty;
        public bool IsAdmin { get; set; }
        public ICollection<CourseTutor> CourseTutors { get; set; } = new List<CourseTutor>();

        public override string ToString()
        {
            return Name + " " + LastName;
        }
    }

   
}
