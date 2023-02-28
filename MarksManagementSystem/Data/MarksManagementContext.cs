using MarksManagementSystem.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace MarksManagementSystem.Data
{
    public class MarksManagementContext : DbContext
    {
        public MarksManagementContext(DbContextOptions<MarksManagementContext> options) : base(options) { }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Course> Courses { get; set; }

        protected override void OnModelCreating (ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Course>().HasIndex(c => c.Name).IsUnique();
            modelBuilder.Entity<Teacher>().HasIndex(c => c.Email).IsUnique();

            modelBuilder.Entity<Teacher>()
                .HasOne<Course>(t => t.CourseLed)
                .WithOne(c => c.HeadTeacher)
                .HasForeignKey<Course>(c => c.HeadTeacherId);
        }
    }
}
