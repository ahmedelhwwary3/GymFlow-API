using GymManagementAPI.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using RepositoryTier.Attendance.DTOs;
using RepositoryTier.Attendance.Enums;
using RepositoryTier.Attendance.Results;
using log=GymManagementAPI.Extensions.IloggerExtensions;
using GymManagementAPI.Extensions;
using RepositoryTier.Coach.Enums;
using RepositoryTier.User.Enums;
using ServiceTier.Attendance; 
using System.Security.Claims;

namespace GymManagementAPI.Controllers
{
    [Route("api/Attendance")]
    [ApiController]
    [Authorize]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceService _attdService;
        private readonly ILogger<AttendanceController> _logger;
        private string _Ip;
        private string _adminId;
        public AttendanceController(IAttendanceService atndService,
            ILogger<AttendanceController> logger)
        {
            _attdService = atndService;
            _logger= logger;
            _adminId = HttpContext.GetAdminId();
            _Ip = HttpContext.GetIPAddress();
        }

        [EnableRateLimiting(Policies.TokenBucketAuthLimiter)]
        [Authorize(Roles =UserRoles.Admin)]
        [HttpGet("Attendances", Name = "GetAttendances")]
        [ProducesResponseType(StatusCodes.Status200OK)] 
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<IEnumerable<GetAttendancesResponse>>>
            GetAttendances([FromQuery] GetAttendancesRequest request)
        {
            bool isMember = User.IsInRole(((int)enUserRole.Member).ToString());
            int? memberId=default;
            if(isMember)
            {
                string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized("Token is no longer valid");

                memberId = Convert.ToInt32(userId);
            }

            if (!ModelState.IsValid)
            {
                _logger.LogAdminInvalidAction(nameof(GetAttendances),
                    log.enInvalidAdminActionReason.InvalidInput,_adminId,_Ip);
                return BadRequest();
            }
               

            var response = await _attdService
                .GetAttendancesAsync(request,memberId);

            return Ok(response);
        }

        [EnableRateLimiting(Policies.SlidingWindowAuthLimiter)]
        [HttpPost(Name = "AddAttendance")]
        [Authorize(Roles = UserRoles.Admin)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<int>>
            AddAttendance([FromBody] AddAttendanceRequest request)
        {
             
            if (request.MemberId is null && string.IsNullOrEmpty(request.Search?.Trim()))
            {
                _logger.LogAdminInvalidAction(nameof(AddAttendance),
                  log.enInvalidAdminActionReason.InvalidInput, _adminId, _Ip); 
                return BadRequest("Identifier is required");
            }
                  
            var response = await _attdService
                .AddAttendanceAsync(request);

            if(response.Status==enAddAttendanceStatus.Succeeded)
            {
                _logger.LogAdminExecutedAction(nameof(AddAttendance), _adminId, _Ip); 
                return Ok(response.Id);
            }

            _logger.LogAdminInvalidAction(nameof(AddAttendance),
                log.enInvalidAdminActionReason.LogicalError, _adminId, _Ip);
            return response.Status switch
            {

                enAddAttendanceStatus.HasFrozenSubscription => BadRequest("Member subscription is frozen"),

                enAddAttendanceStatus.HasExpiredSubscription => BadRequest("Member subscription is expired"),

                enAddAttendanceStatus.HasTodayAttendance => BadRequest("Member has attendance today"),

                enAddAttendanceStatus.MemberNotFound => NotFound("Member not found"),

                _ => StatusCode(StatusCodes.Status500InternalServerError)
            };
        }


    }
}
