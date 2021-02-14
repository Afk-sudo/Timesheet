using System.Collections.Generic;
using Timesheet.Api.Entities;

namespace Timesheet.Api.Repositories
{
    public interface IEmployeeRepository
    {
        List<Employee> Employees { get; }
    }
}