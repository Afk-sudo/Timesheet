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

        public bool TrackTime(TimeLog timeLog)
        {
            bool isValid = timeLog.WorkingHours > 0 
                           && timeLog.WorkingHours <= 24 
                           && _employeeRepository.IsEmployeeExist(timeLog.EmployeeLogin);
            var employee = UserSession.Sessions.FirstOrDefault(e => e.Login == timeLog.EmployeeLogin);
            if (employee == null)
                isValid = false;            

            if(isValid)
            {
                _timeLogRepository.Add(timeLog);
                return true;
            }
            return false;
        }
    }
 }