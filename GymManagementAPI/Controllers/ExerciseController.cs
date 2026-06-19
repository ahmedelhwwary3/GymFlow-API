using GymManagementAPI.Extensions;
using GymManagementAPI.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using RepositoryTier.Exercise.DTOs;
using RepositoryTier.Exercise.Enums;
using ServiceTier.Exercise;
using log = GymManagementAPI.Extensions.IloggerExtensions;


namespace GymManagementAPI.Controllers
{
    [Route("api/Exercise")]
    [ApiController]
    [Authorize]
    public class ExerciseController : ControllerBase
    {
        private readonly IExerciseService _exerciseService;
        private readonly ILogger _logger;
        private string _Ip;
        private string _adminId;
        public ExerciseController(IExerciseService exerciseService,ILogger logger)
        {
            _logger = logger;
            _exerciseService = exerciseService;
            _adminId = HttpContext.GetAdminId();
            _Ip = HttpContext.GetIPAddress();
        }

        [EnableRateLimiting(Policies.TokenBucketAuthLimiter)]
        [AllowAnonymous]
        [HttpGet("Exercises",Name = "GetExercises")] 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)] 
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GetExercisesResponse>> 
            GetExercises([FromQuery] GetExercisesRequest request)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogAdminInvalidAction(nameof(GetExercises),
                    log.enInvalidAdminActionReason.InvalidInput, _adminId, _Ip);
                return BadRequest();
            }
              
            var response = await _exerciseService.GetExercisesAsync(request);
            return Ok(response);
        }

        [EnableRateLimiting(Policies.SlidingWindowAuthLimiter)]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Coach}")]
        [HttpPost(Name = "AddExercise")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>>
            AddExercise([FromBody] AddExerciseRequest request)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogAdminInvalidAction(nameof(AddExercise),
                    log.enInvalidAdminActionReason.InvalidInput,_adminId,_Ip);
                return BadRequest();
            }
                 
            var response = await _exerciseService.AddAsync(request);
            if(response.Status==enAddExerciseStatus.Succeeded)
            {
                _logger.LogAdminExecutedAction(nameof(AddExercise),_adminId,_Ip);
                return Ok(response.Id);
            }

            _logger.LogAdminInvalidAction(nameof(AddExercise),
                log.enInvalidAdminActionReason.LogicalError, _adminId, _Ip);
            return response.Status switch
            {
                enAddExerciseStatus.NotUniqueName => Conflict("Exercise name must be unique"), 

                _ => StatusCode(StatusCodes.Status500InternalServerError)
            };
        }

        [EnableRateLimiting(Policies.SlidingWindowAuthLimiter)]
        [Authorize(Roles = $"{UserRoles.Admin},{UserRoles.Coach}")]
        [HttpPut("{Id}",Name = "UpdateExerciseById")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult>
           UpdateExerciseById(int Id,UpdateExerciseRequest request)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogAdminInvalidAction(nameof(UpdateExerciseById),
                 log.enInvalidAdminActionReason.InvalidInput, _adminId, _Ip);
                return BadRequest();
            }
                
            var status = await _exerciseService.UpdateByIdAsync(Id,request);
            if(status==enUpdateExerciseStatus.Succeeded)
            {
                _logger.LogAdminExecutedAction(nameof(UpdateExerciseById),_adminId,_Ip);
                return NoContent();
            }

            _logger.LogAdminInvalidAction(nameof(UpdateExerciseById),
                log.enInvalidAdminActionReason.LogicalError, _adminId, _Ip);
            return status switch
            {
                enUpdateExerciseStatus.NotUniqueName => Conflict("Exercise name is not unique"),

                enUpdateExerciseStatus.DataNotChanged => BadRequest("Data not changed"),

                enUpdateExerciseStatus.ExerciseNotFound => NotFound("Exercise not found"),

                _ =>StatusCode(StatusCodes.Status500InternalServerError)
            };
        }
    }
}
