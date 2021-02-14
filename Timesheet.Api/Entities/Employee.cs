namespace Timesheet.Api.Entities
{
    public class Employee : BaseEntity
    {
        public string Login { get; set; }
        public string PasswordHash { get; set; }
    }
}