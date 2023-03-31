namespace MarksManagementSystem.ViewModel
{
    public class ViewAllStudentsViewModel
    {
        public int StudentId { get; set; }
        public string StudentFullName { get; set; } = string.Empty;
        public string StudentEmail { get; set; } = string.Empty;
        public string StudentDateOfBirth { get; set; } = string.Empty;
        public string StudentEnrolledCourses { get; set; } = string.Empty;
    }
}
