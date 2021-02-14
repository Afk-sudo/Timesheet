using System.Linq;
using Timesheet.Domain.Abstractions;
using Timesheet.Domain.Entities;
using Timesheet.Domain.Models;

namespace Timesheet.Api.Services
{
    public class ReportService : IReportService
    {
        public ReportService(ITimesheetRepository timesheetRepository)
        {
            _timesheetRepository = timesheetRepository;
        }
        
        private readonly ITimesheetRepository _timesheetRepository;
        public EmployeeReport GetEmployeeReport(string login)
        {
            var timeLogs = _timesheetRepository.GetTimeLogs(login);
            
            return new EmployeeReport
            {
                Employee = new Employee
                {
                    Login = login
                },
                TimeLogs = timeLogs.ToList()
            };
        }
    }
}