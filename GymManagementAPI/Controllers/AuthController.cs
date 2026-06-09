using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceTier;
using auth = RepositoryTier.DTOs.Authentication;
using RepositoryTier.User.DTOs.Authentication;

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
        public async Task<ActionResult<TokenResponse>> Login(LoginRequest request)
        {
            if(!ModelState.IsValid)
                return BadRequest();

            var response = await _userService.LoginAsync(request);
            if (response==null)
                return Unauthorized("Invalid email or password.");

            return Ok(response);
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<TokenResponse>> Refresh(RefreshRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var response = await _userService.RefreshAsync(request);
            if (response == null)
                return Unauthorized("Invalid refresh token.");

            return Ok(response);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout(LogoutRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var response = await _userService.LogoutAsync(request);
            // To confuse attackers, we return 200 OK even if the logout fails (e.g., invalid token)
            if (response == false)
                return Ok();

            return Ok("Logged out successfully");// Frindly message for successful logout
        }

    }
}
