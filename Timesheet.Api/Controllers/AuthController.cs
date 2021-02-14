using Microsoft.AspNetCore.Mvc;
using Timesheet.Api.ResourceModels;
using Timesheet.Api.Services;
using Timesheet.Domain.Abstractions;

namespace Timesheet.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        
        private readonly IAuthService _authService;
    
        [HttpPost]
        public IActionResult Login([FromBody]LoginRequest request)
        {
            return Ok(_authService.Login(request.Login, request.Password));
        }
    }
}