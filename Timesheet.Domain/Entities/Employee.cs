namespace Timesheet.Domain.Entities
{
    public class Employee : BaseEntity
    {
        public Employee() { }
        public Employee(string login, decimal salary)
        {
            Login = login;
            Salary = salary;
        }
        
        public string Login { get; set; }
        public string PasswordHash { get; set; }
        public decimal Salary { get; set; }
    }
}