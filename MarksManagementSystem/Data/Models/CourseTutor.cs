using Microsoft.Identity.Client;

namespace MarksManagementSystem.Data.Models
{
    public class CourseTutor
    {
        public int Id { get; set; } 
        public int CourseId { get; set; }
        public int TutorId { get;set; }
        public Course Course { get; set; } = new Course();
        public Tutor Tutor { get; set;} = new Tutor();
        public bool IsUnitLeader { get; set; }
    }
}
