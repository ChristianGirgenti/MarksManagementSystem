using Microsoft.Identity.Client;

namespace MarksManagementSystem.Data.Models
{
    public class CourseTeacher
    {
        public int Id { get; set; } 
        public int CourseId { get; set; }
        public int TeacherId { get;set; }
        public Course Course { get; set; }
        public Teacher Teacher { get; set;}
        public bool IsHeadTeacher { get; set; }

    }
}
