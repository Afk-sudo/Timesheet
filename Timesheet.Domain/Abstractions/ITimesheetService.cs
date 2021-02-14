using Timesheet.Domain.Models;

namespace Timesheet.Domain.Abstractions
{
    public interface ITimesheetService
    {
        public bool TrackTime(TimeLog timeLog);
    }
}