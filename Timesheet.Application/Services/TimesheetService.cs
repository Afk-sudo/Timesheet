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
        public TimesheetService(ITimesheetRepository timesheetRepository)
        {
            _timesheetRepository = timesheetRepository;
            _employeeRepository = new EmployeeRepository();
        }

        private readonly ITimesheetRepository _timesheetRepository;
        private readonly IEmployeeRepository _employeeRepository;
        public bool TrackTime(TimeLog timeLog)
        {
            bool isValid = timeLog.WorkingHours > 0 
                           && timeLog.WorkingHours <= 24 
                           && _employeeRepository.IsEmployeeExist(timeLog.Employee.Login);
            var employee = UserSession.Sessions.FirstOrDefault(e => e.Login == timeLog.Employee.Login);
            if (employee == null)
                isValid = false;            

            if(isValid)
            {
                _timesheetRepository.Add(timeLog);
                return true;
            }
            return false;
        }
    }
 }