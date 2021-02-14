using System.Collections.Generic;
using Timesheet.Domain.Entities;

namespace Timesheet.Domain.Abstractions
{
    public interface IEmployeeRepository
    {
        List<Employee> Employees { get; }
        bool IsEmployeeExist(string login);
    }
}