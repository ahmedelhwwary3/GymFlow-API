using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryTier.DTOs.Users;
using RepositoryTier.Enums.Users;
using ServiceTier;
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
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
        {
            if(!ModelState.IsValid)
                return BadRequest();

            int userId = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier)); 
            request.UserId = userId;
            enChangePasswordResult result = await _userService.ChangePasswordAsync(request);
             
            return result switch
            {
                enChangePasswordResult.Succeeded =>
                    NoContent(),

                enChangePasswordResult.InvalidConfirmPassword =>
                    BadRequest("Password confirmation does not match."),

                enChangePasswordResult.InvalidCurrentPassword =>
                    BadRequest("Current password is incorrect."),

                enChangePasswordResult.UserNotFound =>
                    NotFound(),

                _ => StatusCode(StatusCodes.Status500InternalServerError)
            };
        }

    }
}
