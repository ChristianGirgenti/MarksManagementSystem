using Microsoft.Identity.Client;

namespace MarksManagementSystem.Data.Models
{
    public class CourseTutor
    {
        public int Id { get; set; } 
        public int CourseId { get; set; }
        public int TutorId { get;set; }
        public Course Course { get; set; }
        public Tutor Tutor { get; set;}
        public bool IsUnitLeader { get; set; }
    }
}
