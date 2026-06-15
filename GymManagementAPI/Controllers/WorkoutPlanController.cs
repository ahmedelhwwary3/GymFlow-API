using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RepositoryTier.Member.Enums;
using RepositoryTier.User.Enums;
using RepositoryTier.WorkoutPlan.DTOs;
using RepositoryTier.WorkoutPlan.Enums;
using RepositoryTier.WorkoutPlan.Results;
using ServiceTier.WorkoutPlan; 
using System.Security.Claims;

namespace GymManagementAPI.Controllers
{
    [Route("api/WorkoutPlan")]
    [ApiController]
    public class WorkoutPlanController : ControllerBase
    {
        private readonly IWorkoutPlanService _workoutPlanService;
        public WorkoutPlanController(IWorkoutPlanService workoutPlanService)
        {
            _workoutPlanService = workoutPlanService;
        }

        [HttpGet("WorkoutPlans",Name = "GetWorkoutPlans")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GetWorkoutPlansResponse>> 
            GetWorkoutPlans([FromQuery] GetWorkoutPlansRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest();

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

        [HttpPost(Name = "AddWorkoutPlan")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AddWorkoutPlanResult>>
            AddWorkoutPlan(AddWorkoutPlanRequest request)
        { 
            if (!ModelState.IsValid)
                return BadRequest();

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

            return result.Status switch
            {
                enAddWorkoutPlanStatus.ExerciseRepeated => BadRequest("An exercise is repeated"),

                enAddWorkoutPlanStatus.ExerciseNotFound => BadRequest("An exercise is not found"),

                enAddWorkoutPlanStatus.CoachNotFound => BadRequest("Coach not found"), 

                enAddWorkoutPlanStatus.CoachInactive => BadRequest("Coach is not active"),

                enAddWorkoutPlanStatus.MemberNotFound => NotFound("Member not found"),

                _ => CreatedAtRoute("GetWorkoutPlanById", new {Id=result.Id},null)
            };
        }

        [HttpGet("{Id}",Name = "GetWorkoutPlanById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]  
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GetWorkoutPlanByIdResponse>> 
            GetWorkoutPlanById(int Id)
        {
            if (Id < 1)
                return BadRequest();
             
            var response = await _workoutPlanService.GetByIdAsync(Id);

            return Ok(response); 
        }
    }
}
