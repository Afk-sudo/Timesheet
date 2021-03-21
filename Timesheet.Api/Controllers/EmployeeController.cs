using Microsoft.AspNetCore.Mvc;
using Timesheet.Api.ResourceModels;
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
        [Route("[action]")]
        public IActionResult AddChief([FromBody] ChiefEmployee employee)
        {
            return Ok(_employeeService.AddChiefEmployee(employee));
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult AddStaff([FromBody] StaffEmployee employee)
        {
            return Ok(_employeeService.AddStaffEmployee(employee));
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult AddFreelancer([FromBody] FreelancerEmployee employee)
        {
            return Ok(_employeeService.AddFreelancerEmployee(employee));
        }
    }
}