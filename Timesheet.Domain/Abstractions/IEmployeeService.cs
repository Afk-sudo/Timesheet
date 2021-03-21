using Timesheet.Domain.Entities;

namespace Timesheet.Domain.Abstractions
{
    public interface IEmployeeService
    {
        bool AddStaffEmployee(StaffEmployee employee);
        bool AddChiefEmployee(ChiefEmployee employee);
        bool AddFreelancerEmployee(FreelancerEmployee employee);
        
    }
}