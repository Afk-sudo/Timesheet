using Timesheet.Domain.Entities;

namespace Timesheet.Domain.Abstractions
{
    public interface IEmployeeRepository
    {
        bool IsEmployeeExist(string login);
        Employee GetEmployee(string login);
        Employee GetEmployeeByLoginPassword(string login, string passwordHash);
        void Add(Employee employee);
    }
}