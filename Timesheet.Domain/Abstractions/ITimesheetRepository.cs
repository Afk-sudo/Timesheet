using System.Collections.Generic;
using Timesheet.Domain.Models;

namespace Timesheet.Domain.Abstractions
{
    public interface ITimesheetRepository
    {
        TimeLog[] GetTimeLogs(string login);
    }
}