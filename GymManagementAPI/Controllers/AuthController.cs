using GymManagementAPI.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using RepositoryTier.Authentication.DTOs;
using RepositoryTier.User.Enums;
using RepositoryTier.User.Results;
using ServiceTier;
using ServiceTier.User;
using System.Net;

namespace GymManagementAPI.Controllers
{
    [Route("api/Auth")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly ILogger<AuthController> _logger;
        private readonly IUserService _userService;
        private string _IPAddress;
                
        public AuthController(
            IUserService userService,
            ILogger<AuthController> logger)
        {
            _userService = userService;
            _logger = logger;
            _IPAddress= HttpContext.Connection.RemoteIpAddress?
                .ToString() ?? "Unknown IP";
        }

        [EnableRateLimiting(Policies.FixedWindowAuthLimiter)]
        [HttpPost("Login",Name = "Login")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<TokenResponse>> Login(LoginRequest request)
        {
            if(!ModelState.IsValid)
                return BadRequest();
              
            var result = await _userService.LoginAsync(request);
            if (result.LoginStatus == enLoginStatus.Succeeded)
            {
                _logger.LogInformation(
                    "Successful login for email={Email}, IP={IPAddress}",
                    request.Email,
                    _IPAddress);

                return Ok(result.TokenResponse);
            }

            _logger.LogWarning("Invalid Login attempt for email={request.Email} , IP={IPAddress}"
                , request.Email, _IPAddress);

            return result.LoginStatus switch
            {
                enLoginStatus.UserNotFound => Unauthorized("Invalid username or password"),

                enLoginStatus.InvalidPassword => Unauthorized("Invalid username or password"),

                enLoginStatus.Inactive => Unauthorized("User is not active"),

                _ => StatusCode(StatusCodes.Status500InternalServerError)
            };
        }

        [EnableRateLimiting(Policies.FixedWindowAuthLimiter)]
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
            if (result.RefreshStatus == enRefreshStatus.Succeeded)
            {
                _logger.LogInformation("Refresh token attempt succeeded for " +
                    "email={request.Email}, IP={_IPAddress}",
                    request.Email, _IPAddress);
                return Ok(result.TokenResponse);
            }

            _logger.LogWarning("Invalid refresh token attempt for email= " +
                "{request.Email} , IP={_IPAddress}",
                request.Email,
                _IPAddress);

            return result.RefreshStatus switch
            {
                enRefreshStatus.Inactive => Unauthorized("User is not active"),

                enRefreshStatus.UserNotFound => NotFound("User is not found"),

                enRefreshStatus.InvalidToken=>Unauthorized("Token is no longer valid"),

                _ => StatusCode(StatusCodes.Status500InternalServerError)
            }; 
        }

        [EnableRateLimiting(Policies.FixedWindowAuthLimiter)]
        [HttpPost("Logout", Name = "Logout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Logout(LogoutRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            
            _logger.LogInformation("Logout attempt for email={request.Email}, IP={_IPAddress}",
                request.Email,_IPAddress);

            bool succeeded = await _userService.LogoutAsync(request);
            //OK() To confuse attackers, we return 200 OK even if the logout fails (e.g., invalid token) 
            return succeeded ? Ok("Logged out successfully") : Ok(); 
        }

    }
}
