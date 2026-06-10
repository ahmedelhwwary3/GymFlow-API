using Microsoft.AspNetCore.Mvc;
using RepositoryTier.Coach.DTOs; 
using ServiceTier.Coach;

namespace GymManagementAPI.Controllers
{
    [Route("api/Coach")]
    [ApiController]
    public class CoachController : ControllerBase
    {
        private readonly ICoachService _coachService;
        public CoachController(ICoachService coachService)
        {
            _coachService= coachService;
        }

        [HttpGet("Coaches",Name = "GetCoaches")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<GetCoachesResponse>>>
            GetCoaches([FromQuery]GetCoachesRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var response = await _coachService
                .GetCoachesAsync(request);

            if (response == null || response.Coaches.Count==0)
                return NotFound("Coaches not found");

            return Ok(response);
        }

        [HttpPost(Name ="Add")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AddCoachResponse>> Add(AddCoachRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var response = await _coachService.AddAsync(request);
            return response!=null? CreatedAtAction("Add",response) :
                StatusCode(StatusCodes.Status500InternalServerError);
        }



         
    }
}
