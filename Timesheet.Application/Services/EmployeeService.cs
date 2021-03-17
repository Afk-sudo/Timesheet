using Timesheet.Domain.Abstractions;
using Timesheet.Domain.Entities;

namespace Timesheet.Application.Services
{
    public class EmployeeService : IEmployeeService
    {
        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        private readonly IEmployeeRepository _employeeRepository;
        
        public bool AddEmployee(Employee employee)
        {
            bool isValid = !string.IsNullOrEmpty(employee.Login) && employee.Salary > 0;

            if (isValid)
            {
                _employeeRepository.Add(employee);
            }

            return isValid;
        }
    }
}