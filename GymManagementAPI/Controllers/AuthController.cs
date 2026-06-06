using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceTier;
using auth= RepositoryTier.DTOs.Authentication;

namespace GymManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IUserService _userService;
        public AuthController(
            IUserService userService,
            ILogger<AuthController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(auth.LoginRequest request)
        {
            if(!ModelState.IsValid)
                return BadRequest();

            var response =await _userService.LoginAsync(request);
            if(response==null)
                return Unauthorized("Invalid email or password.");

            return Ok(response);
        }


    }
}
