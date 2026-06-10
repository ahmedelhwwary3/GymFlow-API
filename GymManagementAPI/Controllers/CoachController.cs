using Microsoft.AspNetCore.Mvc; 

namespace GymManagementAPI.Controllers
{
    [Route("api/Coach")]
    [ApiController]
    public class CoachController : ControllerBase
    { 


        [HttpGet("All",Name ="GetAll")]
        public IEnumerable<string> Get()
        {


            return null;
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
