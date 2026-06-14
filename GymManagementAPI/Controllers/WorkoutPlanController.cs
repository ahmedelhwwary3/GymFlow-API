using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryTier.User.Enums;
using RepositoryTier.WorkoutPlan.DTOs;
using ServiceTier.WorkoutPlan;
using ServiceTier.WorkoutPlanExercise;
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
    }
}
