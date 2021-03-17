using System.Linq;
using Timesheet.Domain.Abstractions;
using Timesheet.Domain.Entities;

namespace Timesheet.DataAccess.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        public EmployeeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        private readonly ApplicationDbContext _context;

        public bool IsEmployeeExist(string login)
        {
            return _context.Employees.Any(e => e.Login == login);
        }

        public Employee GetEmployee(string login)
        {
            return _context.Employees.FirstOrDefault(e => e.Login == login);
        }

        public Employee GetEmployeeByLoginPassword(string login, string passwordHash)
        {
            return _context.Employees
                .FirstOrDefault(e => e.Login == login && e.PasswordHash == passwordHash);
        }

        public void Add(Employee employee)
        {
            _context.Employees.Add(employee);
            _context.SaveChanges();
        }
    }
}