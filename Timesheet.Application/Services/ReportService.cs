using System.Collections.Generic;
using System.Linq;
using Timesheet.Domain.Abstractions;
using Timesheet.Domain.Entities;

namespace Timesheet.Api.Services
{
    public class ReportService : IReportService
    {
        public ReportService(ITimeLogRepository timeLogRepository, 
            IEmployeeRepository employeeRepository)
        {
            _timeLogRepository = timeLogRepository;
            _employeeRepository = employeeRepository;
        }

        private readonly ITimeLogRepository _timeLogRepository;
        private readonly IEmployeeRepository _employeeRepository;
        public EmployeeReport GetEmployeeReport(string login)
        {
            var employee = _employeeRepository.GetEmployee(login);
            var timeLogs = _timeLogRepository.GetTimeLogs(login);

            if (timeLogs == null || timeLogs.Length == 0)
            {
                return new EmployeeReport()
                {
                    Employee = employee,
                    TimeLogs = new List<TimeLog>(),
                    TotalHours = 0,
                    Bill = 0
                };
            }

            decimal bill = employee.CalculateBill(timeLogs);
            
            return new EmployeeReport
            {
                Employee = employee,    
                TimeLogs = timeLogs.ToList(),
                TotalHours = timeLogs.Sum(x => x.WorkingHours),
                Bill = bill
            };
        }
    }
}