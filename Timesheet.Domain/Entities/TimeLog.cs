using System;
using Timesheet.Domain.Entities;

namespace Timesheet.Domain.Entities
{
    public class TimeLog : BaseEntity
    {
        public DateTime Date { get; set; } = new DateTime(1, 2, 3);
        public int WorkingHours { get; set; }
        public Employee Employee { get; set; }
        public string Comment { get; set; }
        
    }
}