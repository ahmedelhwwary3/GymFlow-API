using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryTier.User.DTOs;
using RepositoryTier.User.Enums;
using ServiceTier;
using ServiceTier.User;
using System.Security.Claims;

namespace GymManagementAPI.Controllers
{
    [Route("api/User")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService=userService;
        }

        [HttpPatch(Name = "ChangePassword")] 
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier); 

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Access Token is no longer valid");

            if(!ModelState.IsValid)
                return BadRequest();

            int Id = Convert.ToInt32(userId);
            var result = await _userService.ChangePasswordAsync(Id, request);

            return result switch
            {
                enChangePasswordStatus.Succeeded =>
                    Ok("Password changed successfully"),

                enChangePasswordStatus.OldPassword =>
                    BadRequest("Password is used before"),

                enChangePasswordStatus.InvalidConfirmPassword =>
                    BadRequest("Password confirmation does not match."),

                enChangePasswordStatus.InvalidCurrentPassword =>
                    BadRequest("Current password is incorrect."),

                _=>NotFound()
            };
        }

    }
}
