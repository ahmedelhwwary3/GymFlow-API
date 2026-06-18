using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc; 
using RepositoryTier.User.Enums;
using RepositoryTier.User.Results;
using RepositoryTier.Authentication;
using ServiceTier; 
using ServiceTier.User;
using Microsoft.AspNetCore.Authorization;

namespace GymManagementAPI.Controllers
{
    [Route("api/Auth")]
    [ApiController]
    [AllowAnonymous]
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

                enLoginStatus.Inactive => Unauthorized("User is not active"),

                _=>Ok(result.TokenResponse)
            };
        }

        [HttpPost("Refresh",Name = "Refresh")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)] 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TokenResponse>> Refresh(RefreshRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _userService.RefreshAsync(request);

            return result.RefreshStatus switch
            {
                enRefreshStatus.Inactive => Unauthorized("User is not active"),

                enRefreshStatus.UserNotFound => NotFound("User is not found"),

                enRefreshStatus.InvalidToken=>Unauthorized("Token is no longer valid"),

                _ => Ok(result.TokenResponse)
            }; 
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
            //OK() To confuse attackers, we return 200 OK even if the logout fails (e.g., invalid token) 
            return succeeded ? Ok("Logged out successfully") : Ok(); 
        }

    }
}
