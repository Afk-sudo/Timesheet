using System.Collections.Generic;
using Timesheet.Domain.Abstractions;
using Timesheet.Domain.Models;

namespace Timesheet.DataAccess.Npgsql.Repositories
{
    public class TimesheetRepository : ITimesheetRepository
    {
        public TimeLog[] GetTimeLogs(string login)
        {
            return null;
        }
    }
}