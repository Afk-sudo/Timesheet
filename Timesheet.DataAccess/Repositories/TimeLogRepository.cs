using System.Linq;
using Timesheet.Domain.Abstractions;
using Timesheet.Domain.Entities;

namespace Timesheet.DataAccess.Repositories
{
    public class TimeLogRepository : ITimeLogRepository
    {
        public TimeLogRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        
        private readonly ApplicationDbContext _context;
        
        public TimeLog[] GetTimeLogs(string login)
        {
            return _context.TimeLogs.Where(t => t.Employee.Login == login).ToArray();
        }

        public void Add(TimeLog timeLog)
        {
            _context.TimeLogs.Add(timeLog);
            _context.SaveChanges();
        }
    }
}