using Microsoft.AspNetCore.Components;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MarksManagementSystem.Data.Models
{
    public class Teacher
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name can not be empty")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Last name can not be empty")]
        public string LastName { get; set; }
        [EmailAddress]
        [Required(ErrorMessage = "Insert a valid email")]
        public string Email { get; set; }
        [PasswordPropertyText]
        [Required(ErrorMessage = "Insert a valid password")]
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
        public int CourseLedId { get; set; }

        public Course CourseLed { get; set; }
    }
}
