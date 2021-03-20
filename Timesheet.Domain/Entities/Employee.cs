namespace Timesheet.Domain.Entities
{
    public abstract class Employee : BaseEntity
    {
        public Employee(string login, decimal salary)
        {
            Login = login;
            Salary = salary;
        }
        
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public decimal Salary { get; set; }
        
        protected const decimal MAX_WORKING_HOURS_PER_MOUNT = 160m;
        protected const decimal MAX_WORKING_HOURS_PER_DAY = 8m;

        public abstract decimal CalculateBill(TimeLog[] timeLogs);
    }
}