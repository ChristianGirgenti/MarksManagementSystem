namespace MarksManagementSystem.ViewModel
{
    public class ViewAllCoursesViewModel
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; } = string.Empty;
        public int CourseCredits { get; set; }
        public string UnitLeader { get; set; } = string.Empty;
        public string OtherTutors { get; set; } = string.Empty;
    }
}
