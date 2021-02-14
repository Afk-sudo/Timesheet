using System.Collections.Generic;
using Timesheet.Api.Models;

namespace Timesheet.Api.Services
{
    public class TimesheetService
    {
        public bool TrackTime(TimeLog timeLog)
        {
            Timesheets.TimeLogs.Add(timeLog);
            return true;
        }
    }

    public static class Timesheets
    {
        public static List<TimeLog> TimeLogs { get; set; } = new List<TimeLog>();
    }
}