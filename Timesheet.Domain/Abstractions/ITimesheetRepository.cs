using Timesheet.Domain.Entities;

namespace Timesheet.Domain.Abstractions
{
    public interface ITimesheetRepository
    {
        TimeLog[] GetTimeLogs(string login);
        void Add(TimeLog timeLog);
    }
}