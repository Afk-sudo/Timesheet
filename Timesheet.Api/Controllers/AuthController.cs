using Microsoft.AspNetCore.Mvc;
using Timesheet.Api.ResourceModels;
using Timesheet.Api.Services;

namespace Timesheet.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        public AuthController(AuthService authService)
        {
            _authService = authService;
        }
        
        private readonly AuthService _authService;
    
        [HttpPost]
        public IActionResult Login([FromBody]LoginRequest request)
        {
            return Ok(_authService.Login(request.Login, request.Password));
        }
    }
}