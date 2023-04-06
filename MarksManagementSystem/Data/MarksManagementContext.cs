using MarksManagementSystem.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace MarksManagementSystem.Data
{
    public class MarksManagementContext : DbContext
    {
        public MarksManagementContext(DbContextOptions<MarksManagementContext> options) : base(options) { }
        public DbSet<Tutor> Tutor { get; set; }
        public DbSet<Course> Course { get; set; }
        public DbSet<CourseTutor> CourseTutor { get; set; }
        public DbSet<Student> Student { get; set; }
        public DbSet<CourseStudent> CourseStudent { get; set; }

        protected override void OnModelCreating (ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>().HasIndex(c => c.CourseName).IsUnique();
            modelBuilder.Entity<Tutor>().HasIndex(t => t.TutorEmail).IsUnique();
            modelBuilder.Entity<Student>().HasIndex(s => s.StudentEmail).IsUnique();


            modelBuilder.Entity<CourseTutor>()
                .HasKey(c => new { c.CourseId, c.TutorId });
            
            modelBuilder.Entity<CourseTutor>()
                .HasOne(ct => ct.Course)
                .WithMany(c => c.CourseTutors)
                .HasForeignKey(ct => ct.CourseId);

            modelBuilder.Entity<CourseTutor>()
               .HasOne(ct => ct.Tutor)
               .WithMany(t => t.CourseTutors)
               .HasForeignKey(ct => ct.TutorId);


            modelBuilder.Entity<CourseStudent>()
                .HasKey(c => new { c.CourseId, c.StudentId });

            modelBuilder.Entity<CourseStudent>()
                .HasOne(cs => cs.Course)
                .WithMany(c => c.CourseStudents)
                .HasForeignKey(cs => cs.CourseId);

            modelBuilder.Entity<CourseStudent>()
                .HasOne(cs => cs.Student)
                .WithMany(s => s.CourseStudents)
                .HasForeignKey(cs => cs.StudentId);

        }
    }
}
