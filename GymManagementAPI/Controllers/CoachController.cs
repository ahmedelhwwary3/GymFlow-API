using GymManagementAPI.Extensions;
using GymManagementAPI.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using RepositoryTier.Coach.DTOs;
using RepositoryTier.Coach.Enums;
using RepositoryTier.User.DTOs;
using RepositoryTier.User.Enums;
using ServiceTier.Coach;
using log = GymManagementAPI.Extensions.IloggerExtensions;

namespace GymManagementAPI.Controllers
{
    [Route("api/Coach")]
    [ApiController]
    [Authorize]
    public class CoachController : ControllerBase
    {
        private readonly ICoachService _coachService;
        private readonly ILogger<CoachController> _logger;
        private string _Ip;
        private string _adminId;
        public CoachController(ICoachService coachService, ILogger<CoachController> logger)
        {
            _coachService = coachService;
            _logger = logger;
            _adminId = HttpContext.GetAdminId();
            _Ip = HttpContext.GetIPAddress();
        }

        [EnableRateLimiting(Policies.TokenBucketAuthLimiter)]
        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet("Coaches", Name = "GetCoaches")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<GetCoachesResponse>>>
            GetCoaches([FromQuery] GetCoachesRequest request)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogAdminInvalidAction(nameof(GetCoaches),
                   log.enInvalidAdminActionReason.InvalidInput, _adminId, _Ip);
                return BadRequest();
            }
               

            var response = await _coachService.GetCoachesAsync(request);

            return Ok(response);
        }

        [EnableRateLimiting(Policies.SlidingWindowAuthLimiter)]
        [HttpGet("{Id}", Name = "GetCoachById")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GetCoachByIdResponse>> GetCoachById(int Id,
            [FromServices]IAuthorizationService authService)
        {
            if (Id < 1)
            {
                _logger.LogAdminInvalidAction(nameof(GetCoachById),
                   log.enInvalidAdminActionReason.InvalidInput, _adminId, _Ip);
                return BadRequest();
            }
             
            var authResult = await authService
                .AuthorizeAsync(User,Id,Policies.OwnerOrAdmin);

            if (!authResult.Succeeded)
                return Forbid();

            var response = await _coachService.GetByIdAsync(Id);

            return response == null ? NotFound("Coach is not found") : Ok(response);
        }

        [EnableRateLimiting(Policies.SlidingWindowAuthLimiter)]
        [Authorize(Roles = UserRoles.Admin)]
        [HttpPut("{Id}", Name = "UpdateCoach")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GetCoachByIdResponse>>
            UpdateCoach(int Id, UpdateCoachByIdRequest request)
        {
            if (Id < 1 || !ModelState.IsValid)
            {
                _logger.LogAdminInvalidAction(nameof(UpdateCoach),
                 log.enInvalidAdminActionReason.InvalidInput, _adminId, _Ip);
                return BadRequest();
            }
               
            var status = await _coachService.UpdateAsync(Id, request);

            if(status==enUpdateCoachStatus.Succeeded)
            {
                _logger.LogAdminExecutedAction(nameof(UpdateCoach),_adminId,_Ip);
                return NoContent();
            }

            _logger.LogAdminInvalidAction(nameof(UpdateCoach),
                log.enInvalidAdminActionReason.LogicalError, _adminId, _Ip);

            return status switch
            {
                enUpdateCoachStatus.CoachNotFound => NotFound("Coach not found"),

                enUpdateCoachStatus.NotUniqueEmail => Conflict("Email must be unique"),

                enUpdateCoachStatus.NotUniquePhone => Conflict("Phone must be unique"),

                enUpdateCoachStatus.DataNotChanged => BadRequest("Date not changed"),

                _ =>StatusCode(StatusCodes.Status500InternalServerError)
            };
        }

        [EnableRateLimiting(Policies.TokenBucketAuthLimiter)]
        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet("LookUpCoaches", Name = "GetLookUpCoaches")] 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<CoachLookUpResponse>>>
            GetLookUpCoaches()
        {
            var response = await _coachService.GetLookUpCoachesAsync();
            return Ok(response);
        }

    }
}
