using Microsoft.AspNetCore.Mvc;
using Timesheet.Domain.Abstractions;
using Timesheet.Domain.Entities;

namespace Timesheet.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : Controller
    {
        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }
        private readonly IReportService _reportService;

        [HttpGet]
        public ActionResult<EmployeeReport> Report([FromBody]Employee employee)
        {
            return _reportService.GetEmployeeReport(employee.Login);
        }
    }
}