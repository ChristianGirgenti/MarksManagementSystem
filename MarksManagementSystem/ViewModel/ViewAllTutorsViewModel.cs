namespace MarksManagementSystem.ViewModel
{
    public class ViewAllTutorsViewModel
    {
        public int TutorId { get; set; }
        public string TutorFullName { get; set; } = string.Empty;
        public string TutorEmail { get; set; } = string.Empty;
        public string TutorDateOfBirth { get; set; } = string.Empty;
        public string CourseLed { get; set; } = string.Empty;
        public string OtherCourses { get; set; } = string.Empty;
    }
}
