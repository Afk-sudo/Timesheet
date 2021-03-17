using System.Collections.Generic;
using System.Linq;
using Timesheet.Domain.Abstractions;
using Timesheet.Domain.Entities;

namespace Timesheet.Api.Services
{
    public class ReportService : IReportService
    {
        public ReportService(ITimesheetRepository timesheetRepository, 
            IEmployeeRepository employeeRepository)
        {
            _timesheetRepository = timesheetRepository;
            _employeeRepository = employeeRepository;
        }

        private const decimal MAX_WORKING_HOURS_PER_MOUNT = 160m;
        private const decimal MAX_WORKING_HOURS_PER_DAY = 8m;
        
        private readonly ITimesheetRepository _timesheetRepository;
        private readonly IEmployeeRepository _employeeRepository;
        public EmployeeReport GetEmployeeReport(string login)
        {
            var employee = _employeeRepository.GetEmployee(login);
            var timeLogs = _timesheetRepository.GetTimeLogs(login);

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

            var totalHours = timeLogs.Sum(t => t.WorkingHours);
            decimal totalBill = 0;
            
            var workingHoursGroupByDay = timeLogs
                .GroupBy(t => t.Date.ToShortDateString());

            foreach (var workingLogsPerDay in workingHoursGroupByDay)
            {
                int dayHours = workingLogsPerDay.Sum(x => x.WorkingHours);

                if (dayHours > MAX_WORKING_HOURS_PER_DAY)
                {
                    var overtime = dayHours - MAX_WORKING_HOURS_PER_DAY;
                    totalBill += MAX_WORKING_HOURS_PER_DAY / MAX_WORKING_HOURS_PER_MOUNT * employee.Salary;
                    totalBill += overtime / MAX_WORKING_HOURS_PER_MOUNT * employee.Salary * 2;
                    
                }
                else
                {
                    totalBill += dayHours / MAX_WORKING_HOURS_PER_MOUNT * employee.Salary;
                 }
            }
            
            return new EmployeeReport
            {
                Employee = employee,    
                TimeLogs = timeLogs.ToList(),
                TotalHours = totalHours,
                Bill = totalBill
            };
        }
    }
}