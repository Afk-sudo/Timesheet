using System.Collections.Generic;
using System.Linq;
using Timesheet.Domain.Abstractions;
using Timesheet.Domain.Entities;

namespace Timesheet.Application.Services
{
    public class AuthService : IAuthService
    {
        public AuthService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }
            
        private readonly IEmployeeRepository _employeeRepository;
        public bool Login(string login, string password)
        {
            var employee = _employeeRepository.Employees
                .FirstOrDefault(e => e.Login == login && e.PasswordHash == password);

            if (employee != null)
            {
                UserSession.Sessions.Add(employee);
                return true;
            }

            return false;
        }
    }

    public static class UserSession
    {
        static UserSession()
        {
            Sessions = new HashSet<Employee>();
        }
        public static HashSet<Employee> Sessions { get; set; }
    }
}