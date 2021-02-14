using System.Collections.Generic;
using Timesheet.Api.Entities;

namespace Timesheet.Api.Repositories
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
    }
}