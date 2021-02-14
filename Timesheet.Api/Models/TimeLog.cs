using System;

namespace Timesheet.Api.Models
{
    public class TimeLog
    {
        public DateTime Date { get; set; }
        public int WorkingHourse { get; set; }
        public string Login { get; set; }
    }
}