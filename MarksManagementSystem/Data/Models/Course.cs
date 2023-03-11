using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MarksManagementSystem.Data.Models
{
    public class Course
    {
        public int Id { get; set; }

        [MaxLength(50, ErrorMessage = "The name of the course can be max 50 characters")]
        [Required]
        public string Name { get; set; }
        [Required]
        [Range(1,50, ErrorMessage = "Credits must be between 1 and 50")]
        public int Credits { get; set; }        
        public ICollection<CourseTutor>? CourseTutors { get; set; }
    }
}
