using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RepositoryTier.Attendance.DTOs;
using RepositoryTier.Attendance.Results;
using RepositoryTier.Coach.Enums;
using RepositoryTier.Attendance.Enums;
using RepositoryTier.User.Enums;
using ServiceTier.Attendance;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using GymManagementAPI.Helpers;

namespace GymManagementAPI.Controllers
{
    [Route("api/Attendance")]
    [ApiController]
    [Authorize]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceService _attdService;
        public AttendanceController(IAttendanceService atndService)
        {
            _attdService = atndService;
        }

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
                return BadRequest(); 

            var response = await _attdService
                .GetAttendancesAsync(request,memberId);

            return Ok(response);
        }

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
                return BadRequest("Identifier is required");  

            var response = await _attdService
                .AddAttendanceAsync(request);

            return response.Status switch
            {

                enAddAttendanceStatus.HasFrozenSubscription => BadRequest("Member subscription is frozen"),

                enAddAttendanceStatus.HasExpiredSubscription => BadRequest("Member subscription is expired"),

                enAddAttendanceStatus.HasTodayAttendance => BadRequest("Member has attendance today"),

                enAddAttendanceStatus.MemberNotFound => NotFound("Member not found") ,

                _ => Ok(response.Id)
            };
        }


    }
}
