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





        //// GET api/<CoachController>/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/<CoachController>
        //[HttpPost]
        //public void Post([FromBody] string value)
        //{
        //}

        //// PUT api/<CoachController>/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody] string value)
        //{
        //}

        //// DELETE api/<CoachController>/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
