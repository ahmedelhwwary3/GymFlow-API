using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryTier.Member.DTOs;
using ServiceTier.Member;

namespace GymManagementAPI.Controllers
{
    [Route("api/Member")]
    [ApiController]
    public class MemberController : ControllerBase
    {
        private readonly IMemberService _memberService;
        public MemberController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        [HttpGet("AssignedMembersForCoach", Name = "GetAssignedMembersForCoach")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetAssignedMembersForCoachResponse>>
            GetAssignedMembersForCoach([FromQuery]GetAssignedMembersForCoachRequest request)
        {
            if(!ModelState.IsValid)
                return BadRequest();

            var response = await _memberService
                .GetAssignedMembersForCoachAsync(request);

            if (response == null || response.Count == 0)
                return NotFound("Members not found");

            return Ok(response);
        }

        [HttpGet("Members", Name = "GetMembers")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetAssignedMembersForCoachResponse>>
            GetMembers([FromQuery] GetAssignedMembersForCoachRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var response = await _memberService
                .GetAssignedMembersForCoachAsync(request);

            if (response == null || response.Count == 0)
                return NotFound("Members not found");

            return Ok(response);
        }
    }
}
