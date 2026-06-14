using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryTier.Attendance.DTOs;
using RepositoryTier.User.Enums;
using ServiceTier.Attendance;
using System.Security.Claims;

namespace GymManagementAPI.Controllers
{
    [Route("api/Attendance")]
    [ApiController]
    public class AttendanceController : ControllerBase
    {
        private readonly IAttendanceService _attdService;
        public AttendanceController(IAttendanceService atndService)
        {
            _attdService = atndService;
        }

        [HttpGet("Attendances", Name = "GetAttendances")]
        [ProducesResponseType(StatusCodes.Status200OK)] 
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<IEnumerable<GetAttendancesResponse>>>
            GetAttendances([FromQuery] GetAttendancesRequest request)
        {
            bool isMember = User.IsInRole(enUserRole.Member.ToString());
            int? memberId=default;
            if(isMember)
            {
                string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized();

                memberId = Convert.ToInt32(userId);
            }
            if (!ModelState.IsValid)
                return BadRequest(); // notfound - 500 - IsInRole (Id)

            var response = await _attdService
                .GetAttendancesAsync(request,memberId);

            return Ok(response);
        }


    }
}
