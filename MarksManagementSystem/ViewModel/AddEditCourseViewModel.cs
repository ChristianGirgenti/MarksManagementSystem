using System.ComponentModel.DataAnnotations;

namespace MarksManagementSystem.ViewModel
{
    public class AddEditCourseViewModel
    {
        [MaxLength(50, ErrorMessage = "The name of the course can be max 50 characters")]
        [Required]
        public string Name { get; set; } = string.Empty;
        [Required]
        [Range(1, 50, ErrorMessage = "Credits must be between 1 and 50")]
        public int Credits { get; set; }
        [Required (ErrorMessage = "A unit leader must be selected")]
        public int UnitLeaderId { get; set; }

        public List<string> TutorIds { get; set; } = new List<string> ();
    }
}
