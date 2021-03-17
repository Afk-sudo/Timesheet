using Timesheet.Domain.Entities;

namespace Timesheet.Domain.Abstractions
{
    public interface IEmployeeService
    {
        bool AddEmployee(Employee employee);
    }
}