using Microsoft.EntityFrameworkCore;
using Timesheet.Domain.Entities;

namespace Timesheet.DataAccess.Npgsql
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }
        public DbSet<Employee> Employees { get; set; }
    }
}