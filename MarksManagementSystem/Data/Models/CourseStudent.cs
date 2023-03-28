namespace MarksManagementSystem.Data.Models
{
    public class CourseStudent
    {
        public int CourseId { get; set; }
        public int StudentId { get; set; }
        public Course Course { get; set; } = new Course();
        public Student Student { get; set; } = new Student();
        public int Mark { get; set; }
    }
}
