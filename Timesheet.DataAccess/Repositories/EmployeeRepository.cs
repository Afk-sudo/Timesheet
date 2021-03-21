using System;
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
            var freelancer = _context.FreelancerEmployees.FirstOrDefault(x => x.Login == login);
            if (freelancer != null)
                return freelancer;

            var staff = _context.StaffEmployees.FirstOrDefault(x => x.Login == login);
            if (staff != null)
                return staff;
            
            var chief = _context.ChiefEmployees.FirstOrDefault(x => x.Login == login);
            if (chief != null)
                return chief;

            return null;
        }

        public Employee GetEmployeeByLoginPassword(string login, string passwordHash)
        {
            return _context.Employees
                .FirstOrDefault(e => e.Login == login && e.PasswordHash == passwordHash);
        }

        public void AddStaff(StaffEmployee employee)
        {
            _context.StaffEmployees.Add(employee);
            _context.SaveChanges();
        }

        public void AddChief(ChiefEmployee employee)
        {
            _context.ChiefEmployees.Add(employee);
            _context.SaveChanges();
        }

        public void AddFreelancer(FreelancerEmployee employee)
        {
            _context.FreelancerEmployees.Add(employee);
            _context.SaveChanges();
        }
    }
}