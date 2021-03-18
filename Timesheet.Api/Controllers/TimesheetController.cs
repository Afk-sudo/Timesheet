using Microsoft.AspNetCore.Mvc;
using Timesheet.Domain.Abstractions;
using Timesheet.Domain.Entities;

namespace Timesheet.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimesheetController : Controller
    {
        public TimesheetController(ITimesheetService timesheetService)
        {
            _timesheetService = timesheetService;
        }
        private readonly ITimesheetService _timesheetService;
        
        [HttpPost]
        public IActionResult Add([FromBody]TimeLog timeLog)
        {
            if (_timesheetService.TrackTime(timeLog))
                return Ok(timeLog.Comment);
            return NoContent();
        }
    }
}