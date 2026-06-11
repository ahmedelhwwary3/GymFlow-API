using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryTier.Coach.Enums;
using RepositoryTier.Member.DTOs;
using RepositoryTier.Member.Enums;
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
        public async Task<ActionResult<GetMembersResopnse>>
            GetMembers([FromQuery] GetMembersRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var response = await _memberService
                .GetMembersAsync(request);

            if (response == null || response.Count == 0)
                return NotFound("Members not found");

            return Ok(response);
        }

        [HttpPost(Name = "AddMember")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddMember([FromBody] AddMemberRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var result = await _memberService.AddAsync(request);
            return result.Status switch
            {
                enAddMemberStatus.NotUniqueEmail => BadRequest("Email must be unique"),

                enAddMemberStatus.NotUniquePhone => BadRequest("Phone must be unique"), 

                enAddMemberStatus.CoachInactive => BadRequest("Coach must be active"),

                enAddMemberStatus.CoachNotExists => NotFound("Coach not found"),

                enAddMemberStatus.InternalServerError => StatusCode(StatusCodes.Status500InternalServerError),

                _ => CreatedAtRoute("AddMember", result.NewId) // remember to change the route "GetMemberById"
            };
        }
    }
}
