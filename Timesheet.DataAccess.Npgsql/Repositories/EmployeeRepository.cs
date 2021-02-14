using System.Collections.Generic;
using System.Linq;
using Timesheet.Domain.Abstractions;
using Timesheet.Domain.Entities;

namespace Timesheet.DataAccess.Npgsql.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        public EmployeeRepository()
        {
            Employees = new List<Employee>
            {
                new Employee { Id = 1, Login = "Иванов", PasswordHash = "QWERTY"},
                new Employee { Id = 2, Login = "Петров", PasswordHash = "qwerty123"},
                new Employee { Id = 3, Login = "Сидоров", PasswordHash = "password"},
            };
        }
        
        public List<Employee> Employees { get; private set; }
        public bool IsEmployeeExist(string login)
        {
            var employee = Employees.FirstOrDefault(e => e.Login == login);
            if (employee == null)
                return false;
            return true;
        }
    }
}