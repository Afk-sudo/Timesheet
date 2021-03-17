using Timesheet.Domain.Abstractions;
using Timesheet.Domain.Entities;

namespace Timesheet.DataAccess.Npgsql.Repositories
{
    public class TimesheetRepository : ITimesheetRepository
    {
        public TimeLog[] GetTimeLogs(string login)
        {
            return null;
        }

        public void Add(TimeLog timeLog)
        {
            
        }
    }
}