using GymManagementAPI.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using RepositoryTier.Coach.DTOs;
using RepositoryTier.Coach.Enums;
using RepositoryTier.User.DTOs;
using RepositoryTier.User.Enums;
using ServiceTier.Coach;

namespace GymManagementAPI.Controllers
{
    [Route("api/Coach")]
    [ApiController]
    [Authorize]
    public class CoachController : ControllerBase
    {
        private readonly ICoachService _coachService;
        public CoachController(ICoachService coachService)
        {
            _coachService = coachService;
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
                return BadRequest();

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
                return BadRequest();

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
                return BadRequest();

            var response = await _coachService.UpdateAsync(Id, request);

            return response switch
            {
                enUpdateCoachStatus.CoachNotFound => NotFound("Coach not found"),

                enUpdateCoachStatus.NotUniqueEmail => Conflict("Email must be unique"),

                enUpdateCoachStatus.NotUniquePhone => Conflict("Phone must be unique"),

                enUpdateCoachStatus.DataNotChanged => BadRequest("Date not changed"),

                _ => NoContent()
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
