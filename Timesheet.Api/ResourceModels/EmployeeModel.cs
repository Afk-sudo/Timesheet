using Timesheet.Domain.Entities;

namespace Timesheet.Api.ResourceModels
{
    public class EmployeeModel
    {
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public decimal Salary { get; set; }
        public decimal Bonus { get; set; }
    }
}