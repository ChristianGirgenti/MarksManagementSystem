using MarksManagementSystem.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace MarksManagementSystem.Data
{
    public class MarksManagementContext : DbContext
    {
        public MarksManagementContext(DbContextOptions<MarksManagementContext> options) : base(options) { }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseTeacher> CourseTeachers { get; set; }

        protected override void OnModelCreating (ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>().HasIndex(c => c.Name).IsUnique();
            modelBuilder.Entity<Teacher>().HasIndex(c => c.Email).IsUnique();

            modelBuilder.Entity<CourseTeacher>()
                .HasOne(ct => ct.Course)
                .WithMany(c => c.CourseTeachers)
                .HasForeignKey(ct => ct.CourseId);

            modelBuilder.Entity<CourseTeacher>()
               .HasOne(ct => ct.Teacher)
               .WithMany(t => t.CourseTeachers)
               .HasForeignKey(ct => ct.TeacherId);

        }
    }
}
