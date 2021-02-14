using System.Collections.Generic;
using System.Linq;
using Timesheet.Application.Services;
using Timesheet.DataAccess.Npgsql.Repositories;
using Timesheet.Domain.Abstractions;
using Timesheet.Domain.Models;

namespace Timesheet.Api.Services
{
    public class TimesheetService : ITimesheetService
    {
        public TimesheetService()
        {
            _employeeRepository = new EmployeeRepository();
        }
            
        private readonly IEmployeeRepository _employeeRepository;
        public bool TrackTime(TimeLog timeLog)
        {
            bool isValid = timeLog.WorkingHourse > 0 
                           && timeLog.WorkingHourse <= 24 
                           && _employeeRepository.IsEmployeeExist(timeLog.Employee.Login);
            var employee = UserSession.Sessions.FirstOrDefault(e => e.Login == timeLog.Employee.Login);
            if (employee == null)
                isValid = false;            

            if(isValid)
            {
                Timesheets.TimeLogs.Add(timeLog);
                return true;
            }
            return false;
        }
    }

    public static class Timesheets
    {
        public static List<TimeLog> TimeLogs { get; set; } = new List<TimeLog>();
    }
}