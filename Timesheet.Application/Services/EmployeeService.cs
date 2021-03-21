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
        
        public bool AddStaffEmployee(StaffEmployee employee)
        {
            bool isValid = !string.IsNullOrEmpty(employee.Login) && employee.Salary > 0;

            if (isValid)
            {
                _employeeRepository.AddStaff(employee);
            }

            return isValid;
        }

        public bool AddChiefEmployee(ChiefEmployee employee)
        {
            bool isValid = !string.IsNullOrEmpty(employee.Login) 
                           && employee.Salary > 0 
                           && employee.Bonus > 0;

            if (isValid)
            {
                _employeeRepository.AddChief(employee);
            }

            return isValid;
        }
        
        public bool AddFreelancerEmployee(FreelancerEmployee employee)
        {
            bool isValid = !string.IsNullOrEmpty(employee.Login) && employee.Salary > 0;

            if (isValid)
            {
                _employeeRepository.AddFreelancer(employee);
            }

            return isValid;
        }
    }
}