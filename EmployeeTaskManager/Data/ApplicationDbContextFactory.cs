using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EmployeeTaskManager.Data
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseMySql(
                "server=localhost;port=3306;database=employee_task_db;user=root;password=;",
                ServerVersion.AutoDetect("server=localhost;port=3306;database=employee_task_db;user=root;password=;")
            );

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}
