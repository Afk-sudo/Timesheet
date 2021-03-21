using Microsoft.EntityFrameworkCore;
using Timesheet.Domain.Entities;

namespace Timesheet.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<ChiefEmployee> ChiefEmployees { get; set; }
        public DbSet<StaffEmployee> StaffEmployees { get; set; }
        public DbSet<FreelancerEmployee> FreelancerEmployees { get; set; }
        public DbSet<TimeLog> TimeLogs { get; set; }
    }
}