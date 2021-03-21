using System.Linq;

namespace Timesheet.Domain.Entities
{
    public class FreelancerEmployee : Employee
    {
        public FreelancerEmployee(string login, decimal salary)
            : base(login, salary) { }
        
        public override decimal CalculateBill(TimeLog[] timeLogs)
        {
            var totalHours = timeLogs.Sum(x => x.WorkingHours);
            return totalHours * Salary;
        }
    }
}