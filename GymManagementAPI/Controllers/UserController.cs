using Azure;
using GymManagementAPI.Extensions;
using GymManagementAPI.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using RepositoryTier.Member.DTOs;
using RepositoryTier.Member.Enums;
using RepositoryTier.User.DTOs;
using RepositoryTier.User.Enums;
using ServiceTier;
using ServiceTier.User;
using System.Security.Claims;
using log = GymManagementAPI.Extensions.IloggerExtensions;

namespace GymManagementAPI.Controllers
{
    [Route("api/User")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;
        private string _Ip;
        private string _adminId;
        public UserController(
            IUserService userService,
            ILogger<UserController> logger)
        {
            _userService=userService;
            _logger = logger;
            _adminId = HttpContext.GetAdminId();
            _Ip = HttpContext.GetIPAddress();
        }

        [EnableRateLimiting(Policies.FixedWindowAuthLimiter)]
        [HttpPatch(Name = "ChangePassword")] 
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)] 
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier); 

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Access Token is no longer valid");

            if(!ModelState.IsValid)
            {
                _logger.LogAdminInvalidAction(nameof(ChangePassword),
                    log.enInvalidAdminActionReason.InvalidInput, _adminId, _Ip);
                return BadRequest();
            }

            int Id = Convert.ToInt32(userId);
            var status = await _userService.ChangePasswordAsync(Id, request);

            if(status==enChangePasswordStatus.Succeeded)
            {
                _logger.LogAdminExecutedAction(nameof(ChangePassword),_adminId,_Ip);
                return NoContent();
            }

            _logger.LogAdminInvalidAction(nameof(ChangePassword),
             log.enInvalidAdminActionReason.LogicalError, _adminId, _Ip);
            return status switch
            { 
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
            {
                _logger.LogAdminInvalidAction(nameof(RegisterMember),
                    log.enInvalidAdminActionReason.InvalidInput, _adminId, _Ip);
                return BadRequest();
            }
                
            var response = await _userService.RegitserMemberAsync(request);
            if (response.Status == enRegisterMemberStatus.Succeeded)
            {
                _logger.LogAdminExecutedAction(nameof(RegisterMember), _adminId, _Ip);
                return CreatedAtRoute("GetMemeberById", new { Id = response.Id }, null);
            }

            _logger.LogAdminInvalidAction(nameof(RegisterMember),
                   log.enInvalidAdminActionReason.LogicalError, _adminId, _Ip);
            return response.Status switch
            {
                enRegisterMemberStatus.NotUniqueEmail => Conflict("Email must be unique"),

                enRegisterMemberStatus.NotUniquePhone => Conflict("Phone must be unique"),

                enRegisterMemberStatus.CoachInactive => BadRequest("Coach must be active"),

                enRegisterMemberStatus.CoachNotExists => NotFound("Coach not found"),

                _ => StatusCode(StatusCodes.Status500InternalServerError)
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
            {
                _logger.LogAdminInvalidAction(nameof(RegisterCoach),
                 log.enInvalidAdminActionReason.InvalidInput, _adminId, _Ip);
                return BadRequest();
            }

            var result = await _userService.RegitserCoachAsync(request);
            if (result.Status == enRegisterCoachStatus.Succeeded)
            {
                _logger.LogAdminExecutedAction(nameof(RegisterCoach), _adminId, _Ip);
                return CreatedAtRoute("GetCoachById", new { Id = result.Resopnse.Id }, null);
            }

            _logger.LogAdminInvalidAction(nameof(RegisterCoach),
                   log.enInvalidAdminActionReason.LogicalError, _adminId, _Ip);
            return result.Status switch
            {

                enRegisterCoachStatus.NotUniqueEmail => Conflict("Email must be unique"),

                enRegisterCoachStatus.NotUniquePhone => Conflict("Phone must be unique"),

                _ => StatusCode(StatusCodes.Status500InternalServerError)
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
            {
                _logger.LogAdminInvalidAction(nameof(RegisterAdmin),
                log.enInvalidAdminActionReason.InvalidInput, _adminId, _Ip);
                return BadRequest();
            }

            var result = await _userService.RegitserAdminAsync(request);
            if (result.Status == enRegisterAdminStatus.Succeeded)
            {
                _logger.LogAdminExecutedAction(nameof(RegisterAdmin), _adminId, _Ip);
                return CreatedAtRoute("GetUserById", new { Id = result.Id }, null);
            }

            _logger.LogAdminInvalidAction(nameof(RegisterAdmin),
                   log.enInvalidAdminActionReason.LogicalError, _adminId, _Ip);
            return result.Status switch
            {

                enRegisterAdminStatus.NotUniqueEmail => Conflict("Email must be unique"),

                enRegisterAdminStatus.NotUniquePhone => Conflict("Phone must be unique"), 

                _ => StatusCode(StatusCodes.Status500InternalServerError)
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
            {
                _logger.LogAdminInvalidAction(nameof(GetUserById),
                log.enInvalidAdminActionReason.InvalidInput, _adminId, _Ip);
                return BadRequest();
            }

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
            {
                _logger.LogAdminInvalidAction(nameof(UpdateUserById),
                log.enInvalidAdminActionReason.InvalidInput, _adminId, _Ip);
                return BadRequest();
            }

            var status = await _userService.UpdateUserByIdAsync(Id, request);
            if (status == enUpdateUserStatus.Succeeded)
            {
                _logger.LogAdminExecutedAction(nameof(UpdateUserById), _adminId, _Ip);
                return NoContent();
            }

            _logger.LogAdminInvalidAction(nameof(UpdateUserById),
                   log.enInvalidAdminActionReason.LogicalError, _adminId, _Ip);
            return status switch
            {
                enUpdateUserStatus.NotUniqueEmail => Conflict("Email must be unique"),

                enUpdateUserStatus.NotUniquePhone => Conflict("Phone must be unique"), 

                enUpdateUserStatus.DataNotChanged => BadRequest("Data not changed"),

                enUpdateUserStatus.UserNotFound => NotFound("User not found"), 

                _ => StatusCode(StatusCodes.Status500InternalServerError)
            };
        }

    }
}
