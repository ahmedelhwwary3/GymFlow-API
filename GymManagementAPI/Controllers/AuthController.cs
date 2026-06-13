using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryTier.User.DTOs.Authentication;
using RepositoryTier.User.Enums;
using RepositoryTier.User.Results;
using ServiceTier; 
using ServiceTier.User;

namespace GymManagementAPI.Controllers
{
    [Route("api/Auth")]
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

        [HttpPost("Login",Name = "Login")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TokenResponse>> Login(LoginRequest request)
        {
            if(!ModelState.IsValid)
                return BadRequest();

            LoginResult result = await _userService.LoginAsync(request);
            return result.LoginStatus switch
            {
                enLoginStatus.UserNotFound => Unauthorized("Invalid username or password"),

                enLoginStatus.InvalidPassword => Unauthorized("Invalid username or password"),

                enLoginStatus.Deleted => Unauthorized("User is deleted"),

                enLoginStatus.Inactive => Unauthorized("User is not active"),

                _=>Ok(result.TokenResponse)
            };
        }

        [HttpPost("Refresh",Name = "Refresh")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)] 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TokenResponse>> Refresh(RefreshRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _userService.RefreshAsync(request);

            return result.RefreshStatus == enRefreshStatus.Succeeded ?
                Ok(result.TokenResponse) : Unauthorized("Refresh token is no longer valid");
        }

        [HttpPost("Logout", Name = "Logout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Logout(LogoutRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            bool succeeded = await _userService.LogoutAsync(request);
            // To confuse attackers, we return 200 OK even if the logout fails (e.g., invalid token)
            if (!succeeded)
                return Ok();

            return Ok("Logged out successfully");// Frindly message for successful logout
        }

    }
}
