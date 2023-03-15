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

        protected override void OnModelCreating (ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>().HasIndex(c => c.CourseName).IsUnique();
            modelBuilder.Entity<Tutor>().HasIndex(c => c.TutorEmail).IsUnique();

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

        }
    }
}
