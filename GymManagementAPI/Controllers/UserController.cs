using GymManagementAPI.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using RepositoryTier.Member.DTOs;
using RepositoryTier.Member.Enums;
using RepositoryTier.User.DTOs;
using RepositoryTier.User.Enums;
using ServiceTier;
using ServiceTier.User;
using System.Security.Claims;

namespace GymManagementAPI.Controllers
{
    [Route("api/User")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService=userService;
        }

        [EnableRateLimiting(Policies.FixedWindowAuthLimiter)]
        [HttpPatch(Name = "ChangePassword")] 
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)] 
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

        [EnableRateLimiting(Policies.SlidingWindowAuthLimiter)]
        [Authorize(Roles =UserRoles.Admin)] 
        [HttpPost("Member", Name = "RegisterMember")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)] 
        public async Task<ActionResult<int>>
            RegisterMember([FromBody] RegisterMemberRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _userService.RegitserMemberAsync(request);
            return result.Status switch
            {
                enRegisterMemberStatus.NotUniqueEmail => Conflict("Email must be unique"),

                enRegisterMemberStatus.NotUniquePhone => Conflict("Phone must be unique"),

                enRegisterMemberStatus.CoachInactive => BadRequest("Coach must be active"),

                enRegisterMemberStatus.CoachNotExists => NotFound("Coach not found"),

                _ => CreatedAtRoute("GetMemeberById", new { Id = result.Id }, null)
            };
        }

        [EnableRateLimiting(Policies.SlidingWindowAuthLimiter)]
        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost("Coach", Name = "RegisterCoach")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<RegisterCoachResponse>> 
            RegisterCoach(RegisterCoachRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _userService.RegitserCoachAsync(request);
            return result.Status switch
            {

                enRegisterCoachStatus.NotUniqueEmail => Conflict("Email must be unique"),

                enRegisterCoachStatus.NotUniquePhone => Conflict("Phone must be unique"),

                _ => CreatedAtRoute("GetCoachById", new { Id = result.Resopnse.Id }, result.Resopnse)
            };
        }

        [EnableRateLimiting(Policies.SlidingWindowAuthLimiter)]
        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost("Admin", Name = "RegisterAdmin")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>>
            RegisterAdmin(RegisterAdminRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _userService.RegitserAdminAsync(request);
            return result.Status switch
            {

                enRegisterAdminStatus.NotUniqueEmail => Conflict("Email must be unique"),

                enRegisterAdminStatus.NotUniquePhone => Conflict("Phone must be unique"), 

                _ => CreatedAtRoute("GetUserById", new { Id = result.Id },null)
            };
        }

        [EnableRateLimiting(Policies.SlidingWindowAuthLimiter)]
        [HttpGet("{Id}", Name = "GetUserById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GetUserByIdResponse>>
            GetUserById(int Id, [FromServices]IAuthorizationService authSerivce)
        {
            if (Id < 1)
                return BadRequest();

            var authResult = await authSerivce
                .AuthorizeAsync(User,Id,Policies.OwnerOrAdmin);

            if (!authResult.Succeeded)
                return Forbid();

            var response = await _userService.GetUserByIdAsync(Id); 
            return response == null ? NotFound("User is not found") : Ok(response);
        }

        [EnableRateLimiting(Policies.SlidingWindowAuthLimiter)]
        [Authorize(Roles = UserRoles.Admin)]
        [HttpPut("{Id}", Name = "UpdateUserById")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status409Conflict)] 
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateUserById(int Id, [FromBody] UpdateUserRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var status = await _userService.UpdateUserByIdAsync(Id, request);
            return status switch
            {
                enUpdateUserStatus.NotUniqueEmail => Conflict("Email must be unique"),

                enUpdateUserStatus.NotUniquePhone => Conflict("Phone must be unique"), 

                enUpdateUserStatus.DataNotChanged => BadRequest("Data not changed"),

                enUpdateUserStatus.UserNotFound => NotFound("User not found"), 

                _ => NoContent()
            };
        }

    }
}
