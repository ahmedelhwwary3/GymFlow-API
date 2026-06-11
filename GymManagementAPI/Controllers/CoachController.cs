using Microsoft.AspNetCore.Mvc;
using RepositoryTier.Coach.DTOs; 
using ServiceTier.Coach;
using RepositoryTier.Coach.Enums;

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

        [HttpPost(Name ="AddCoach")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AddCoachResponse>> AddCoach(AddCoachRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _coachService.AddAsync(request);
            return result.Status switch {  

                enAddCoachStatus.NotUniqueEmail => BadRequest("Email must be unique"),

                enAddCoachStatus.NotUniquePhone => BadRequest("Phone must be unique"),

                enAddCoachStatus.InternalServerError => StatusCode(StatusCodes.Status500InternalServerError),

                _ => CreatedAtRoute("GetCoachById", result.Resopnse)
            };
        }
         
        [HttpGet("{Id}",Name = "GetCoachById")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<GetCoachByIdResponse>>GetCoachById(int Id)
        {
            if (Id < 1)
                return BadRequest();

            var response = await _coachService.GetByIdAsync(Id);
            return response==null?NotFound("Coach not found"): Ok(response);
        }

        [HttpPut("{Id}", Name = "UpdateCoachById")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<GetCoachByIdResponse>>
            UpdateCoachById(int Id,UpdateCoachByIdRequest request)
        {
            if (Id < 1 || !ModelState.IsValid)
                return BadRequest();

            var response = await _coachService
                .UpdateByIdAsync(Id,request);

            return response switch
            {
                enUpdateCoachByIdStatus.CoachNotFound => NotFound("Coach not found"),

                enUpdateCoachByIdStatus.NotUniqueEmail => BadRequest("Email must be unique"),

                enUpdateCoachByIdStatus.NotUniquePhone => BadRequest("Phone must be unique"),

                enUpdateCoachByIdStatus.DataNotChanged => BadRequest("Date not changed"),

                _ => NoContent()
            };
        } 

    }
}
