using GymManagementAPI.Extensions;
using GymManagementAPI.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using RepositoryTier.Member.Enums;
using RepositoryTier.User.Enums;
using RepositoryTier.WorkoutPlan.DTOs;
using RepositoryTier.WorkoutPlan.Enums;
using RepositoryTier.WorkoutPlan.Results;
using ServiceTier.WorkoutPlan; 
using System.Security.Claims;
using log = GymManagementAPI.Extensions.IloggerExtensions;

namespace GymManagementAPI.Controllers
{
    [Route("api/WorkoutPlan")]
    [ApiController]
    [Authorize]
    public class WorkoutPlanController : ControllerBase
    {
        private readonly IWorkoutPlanService _workoutPlanService;
        private readonly ILogger<WorkoutPlanController> _logger;
        private string _Ip;
        private string _adminId;
        public WorkoutPlanController(
            IWorkoutPlanService workoutPlanService
            , ILogger<WorkoutPlanController> logger)
        {
            _workoutPlanService = workoutPlanService;
            _logger = logger;
            _Ip = HttpContext.GetIPAddress();
            _adminId = HttpContext.GetAdminId();
        }

        [EnableRateLimiting(Policies.TokenBucketAuthLimiter)]
        [Authorize(Roles =$"{UserRoles.Admin}")]
        [HttpGet("WorkoutPlans",Name = "GetWorkoutPlans")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GetWorkoutPlansResponse>> 
            GetWorkoutPlans([FromQuery] GetWorkoutPlansRequest request)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogAdminInvalidAction(nameof(GetWorkoutPlans),
                   log.enInvalidAdminActionReason.InvalidInput, _adminId, _Ip);
                return BadRequest();
            }

            int? memberId = default;
            bool isMember = User.IsInRole(((int)enUserRole.Member).ToString());
            if (isMember)
            {
                string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized("Token is no longe valid");
                memberId = Convert.ToInt32(userId);
            }
            var response = await _workoutPlanService
                .GetWorkoutPlansAsync(request, memberId);

            return Ok(response);
        }

        [EnableRateLimiting(Policies.SlidingWindowAuthLimiter)]
        [Authorize(Roles=$"{UserRoles.Admin},{UserRoles.Coach}")] 
        [HttpPost(Name = "AddWorkoutPlan")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AddWorkoutPlanResult>>
            AddWorkoutPlan(AddWorkoutPlanRequest request)
        { 
            if (!ModelState.IsValid)
            {
                _logger.LogAdminInvalidAction(nameof(AddWorkoutPlan),
                  log.enInvalidAdminActionReason.InvalidInput, _adminId, _Ip);
                return BadRequest();
            }

            bool isAdmin = User.IsInRole(((int)enUserRole.Admin).ToString());
            if (isAdmin && request.CoachId == null)
                return BadRequest("Admin must select coach");

            bool isCoach = User.IsInRole(((int)enUserRole.Coach).ToString());
            if (isCoach)
            {
                string? coachId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(coachId))
                    return Unauthorized("Token is no longe valid");
                request.CoachId = Convert.ToInt32(coachId);
            } 
            //Here we are sure that coachId is not null
            var result = await _workoutPlanService
                .AddFullPlanAsync(request);

            if(result.Status==enAddWorkoutPlanStatus.Succeeded)
            {
                _logger.LogAdminExecutedAction(nameof(AddWorkoutPlan),_adminId,_Ip);
                return CreatedAtRoute("GetWorkoutPlanById", new { Id = result.Id }, null);
            }

            _logger.LogAdminInvalidAction(nameof(AddWorkoutPlan),
               log.enInvalidAdminActionReason.LogicalError, _adminId, _Ip);
            return result.Status switch
            {
                enAddWorkoutPlanStatus.ExerciseRepeated => BadRequest("An exercise is repeated"),

                enAddWorkoutPlanStatus.ExerciseNotFound => BadRequest("An exercise is not found"),

                enAddWorkoutPlanStatus.CoachNotFound => NotFound("Coach not found"), 

                enAddWorkoutPlanStatus.CoachInactive => Unauthorized("Coach is not active"),

                enAddWorkoutPlanStatus.MemberNotFound => NotFound("Member not found"),

                _ => StatusCode(StatusCodes.Status500InternalServerError)
            };
        }

        [EnableRateLimiting(Policies.SlidingWindowAuthLimiter)]
        [HttpGet("{Id}",Name = "GetWorkoutPlanById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GetWorkoutPlanByIdResponse>> 
            GetWorkoutPlanById(int Id, [FromServices]IAuthorizationService authService)
        {
            if (Id < 1)
            {
                _logger.LogAdminInvalidAction(nameof(GetWorkoutPlanById),
                  log.enInvalidAdminActionReason.InvalidInput, _adminId, _Ip);
                return BadRequest();
            }
                 
            var authResult = await authService.AuthorizeAsync(User,Id,Policies.WorkoutPlanOwnerOrAdmin);

            if (!authResult.Succeeded)
                return Forbid();

            var response = await _workoutPlanService.GetByIdAsync(Id);

            return Ok(response); 
        }
    }
}
