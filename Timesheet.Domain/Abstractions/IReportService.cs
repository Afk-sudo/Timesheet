using Timesheet.Domain.Models;

namespace Timesheet.Domain.Abstractions
{
    public interface IReportService
    {
        EmployeeReport GetEmployeeReport(string login);
    }
}