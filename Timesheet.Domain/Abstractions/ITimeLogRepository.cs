using Timesheet.Domain.Entities;

namespace Timesheet.Domain.Abstractions
{
    public interface ITimeLogRepository
    {
        TimeLog[] GetTimeLogs(string login);
        void Add(TimeLog timeLog);
    }
}