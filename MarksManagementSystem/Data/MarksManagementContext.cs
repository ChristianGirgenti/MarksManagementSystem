using MarksManagementSystem.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace MarksManagementSystem.Data
{
    public class MarksManagementContext : DbContext
    {
        public MarksManagementContext(DbContextOptions<MarksManagementContext> options) : base(options) { }
        public DbSet<Teacher> Teachers { get; set; }
    }
}
