using System.Linq;

namespace Timesheet.Domain.Entities
{
    public class StaffEmployee : Employee
    {
        public StaffEmployee(string login, decimal salary)
            : base(login, salary) { }

        public override decimal CalculateBill(TimeLog[] timeLogs)
        {
            var totalHours = timeLogs.Sum(t => t.WorkingHours);
            decimal bill = 0;

            var workingHoursGroupByDay = timeLogs.GroupBy(x =>
                x.Date.ToShortDateString());
            
            foreach (var logPerDay in workingHoursGroupByDay)
            {
                int dayHours = logPerDay.Sum(x => x.WorkingHours);

                if (dayHours > MAX_WORKING_HOURS_PER_DAY)
                {
                    var overtime = dayHours - MAX_WORKING_HOURS_PER_DAY;
                    
                    bill += MAX_WORKING_HOURS_PER_DAY / MAX_WORKING_HOURS_PER_MOUNT * Salary;
                    bill += overtime / MAX_WORKING_HOURS_PER_MOUNT * Salary;
                }
                else
                {
                    bill += dayHours / MAX_WORKING_HOURS_PER_MOUNT * Salary;
                }
            }

            return bill;
        }
    }
}