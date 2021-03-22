using System;
using System.Linq;
using Timesheet.Application.Services;
using Timesheet.Domain.Abstractions;
using Timesheet.Domain.Entities;

namespace Timesheet.Api.Services
{
    public class TimesheetService : ITimesheetService
    {
        public TimesheetService(ITimeLogRepository timeLogRepository, 
            IEmployeeRepository employeeRepository)
        {
            _timeLogRepository = timeLogRepository;
            _employeeRepository = employeeRepository;
        }

        private readonly ITimeLogRepository _timeLogRepository;
        private readonly IEmployeeRepository _employeeRepository;

        public bool TrackTime(TimeLog timeLog, string employeeLogin)
        {
            bool isValid = timeLog.WorkingHours > 0
                           && timeLog.WorkingHours <= 24;
            
            var authorizedEmployee = _employeeRepository.GetEmployee(employeeLogin);
            
            if(isValid == false || authorizedEmployee == null)
            {
                return false;
            }

            if (authorizedEmployee is FreelancerEmployee)
            {
                if (timeLog.Date > DateTime.Now.AddDays(2)
                    || timeLog.Date < DateTime.Now.AddDays(-2))
                {
                    return false;
                }
            }

            if (authorizedEmployee is FreelancerEmployee || authorizedEmployee is StaffEmployee)
            {
                if (authorizedEmployee.Login != timeLog.EmployeeLogin)
                {
                    return false;
                }
            }
            
            _timeLogRepository.Add(timeLog);
            return true;
        }
    }
 }