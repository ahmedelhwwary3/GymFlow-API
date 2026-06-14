using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryTier.Exercise.DTOs;
using ServiceTier.Exercise;

namespace GymManagementAPI.Controllers
{
    [Route("api/Exercise")]
    [ApiController]
    public class ExerciseController : ControllerBase
    {
        private readonly IExerciseService _exerciseService;
        public ExerciseController(IExerciseService exerciseService)
        {
            _exerciseService = exerciseService;
        }

        [HttpGet("Exercises",Name = "GetExercises")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GetExercisesResponse>> 
            GetExercises([FromQuery] GetExercisesRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var response = await _exerciseService.GetExercisesAsync(request);
            return Ok(response);
        }
    }
}
