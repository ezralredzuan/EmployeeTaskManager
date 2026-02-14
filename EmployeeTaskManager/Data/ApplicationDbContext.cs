using EmployeeTaskManager.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeTaskManager.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<TaskItem> Tasks { get; set; }
        public DbSet<TaskAudit> TaskAudits { get; set; }

    }
}
