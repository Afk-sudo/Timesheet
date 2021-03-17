using Microsoft.EntityFrameworkCore;
using Timesheet.Domain.Entities;

namespace Timesheet.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<Employee> Employees { get; set; }
    }
}