using Microsoft.AspNetCore.Mvc;
using Timesheet.Domain.Abstractions;
using Timesheet.Domain.Entities;

namespace Timesheet.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : Controller
    {
        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }
        private readonly IEmployeeService _employeeService;

        [HttpPost]
        public IActionResult AddEmployee([FromBody]Employee employee)
        {
            return Ok(_employeeService.AddEmployee(employee));
        }
    }
}