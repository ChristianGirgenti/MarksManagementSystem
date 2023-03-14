using MarksManagementSystem.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace MarksManagementSystem.Data
{
    public class MarksManagementContext : DbContext
    {
        public MarksManagementContext(DbContextOptions<MarksManagementContext> options) : base(options) { }
        public DbSet<Tutor> Tutors { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseTutor> CourseTutors { get; set; }

        protected override void OnModelCreating (ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>().HasIndex(c => c.Name).IsUnique();
            modelBuilder.Entity<Tutor>().HasIndex(c => c.Email).IsUnique();

            modelBuilder.Entity<CourseTutor>()
                .HasOne(ct => ct.Course)
                .WithMany(c => c.CourseTutors)
                .HasForeignKey(ct => ct.CourseId);

            modelBuilder.Entity<CourseTutor>()
               .HasOne(ct => ct.Tutor)
               .WithMany(t => t.CourseTutors)
               .HasForeignKey(ct => ct.TutorId);

        }
    }
}
