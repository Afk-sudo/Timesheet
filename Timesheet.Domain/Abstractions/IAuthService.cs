namespace Timesheet.Domain.Abstractions
{
    public interface IAuthService
    {
        bool Login(string login, string password);
    }
}