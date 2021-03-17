using Timesheet.Domain.Entities;

namespace Timesheet.Domain.Abstractions
{
    public interface IReportService
    {
        EmployeeReport GetEmployeeReport(string login);
    }
}