using System;
using System.Collections.Generic;
using Timesheet.Domain.Entities;

namespace Timesheet.Domain.Entities
{
    public class EmployeeReport
    {
        public Employee Employee { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        
        public List<TimeLog> TimeLogs { get; set; }
        public int TotalHours { get; set; }
        public decimal Bill { get;set; }
    }
}