using Timesheet.Domain.Entities;

namespace Timesheet.Domain.Abstractions
{
    public interface ITimesheetService
    {
        public bool TrackTime(TimeLog timeLog);
    }
}