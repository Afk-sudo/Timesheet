using Microsoft.AspNetCore.Mvc;
using Timesheet.Domain.Abstractions;
using Timesheet.Domain.Models;

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

        [HttpGet]
        public string Test()
        {
            return "test";
        }
        
        [HttpPost]
        public IActionResult Add([FromBody]TimeLog timeLog)
        {
            if (_timesheetService.TrackTime(timeLog))
                return Ok(timeLog.Comment);
            return NoContent();
        }
    }
}