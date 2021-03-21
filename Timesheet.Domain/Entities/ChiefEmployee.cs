using System.Linq;

namespace Timesheet.Domain.Entities
{
    public class ChiefEmployee : Employee
    {
        public ChiefEmployee(string login, decimal salary, decimal bonus) 
            : base(login, salary)
        {
            Bonus = bonus;
        }

        public decimal Bonus { get; set; }
        
        public override decimal CalculateBill(TimeLog[] timeLogs)
        {
            var totalHours = timeLogs.Sum(x => x.WorkingHours);
            decimal bill = 0;

            var workingHoursByDay = timeLogs
                .GroupBy(x => x.Date.ToShortDateString());

            foreach (var logPerDay in workingHoursByDay)
            {
                var dayHours = logPerDay.Sum(x => x.WorkingHours);

                if (dayHours > MAX_WORKING_HOURS_PER_DAY)
                {
                    decimal bonus = MAX_WORKING_HOURS_PER_DAY / MAX_WORKING_HOURS_PER_MOUNT * Bonus;
                    bill += MAX_WORKING_HOURS_PER_DAY / MAX_WORKING_HOURS_PER_MOUNT * Salary;
                    bill += bonus;
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