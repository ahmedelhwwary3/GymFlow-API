using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RepositoryTier.Coach.Enums;
using RepositoryTier.Member.DTOs;
using RepositoryTier.Member.Enums;
using ServiceTier.Member;
using System.Security.Claims;

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

        [HttpGet(Name = "GetMemeberProfile")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)] 
        public async Task<ActionResult<GetMemberByIdResopnse>>
            GetMemeberProfile()
        {
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized("Token in no longer valid");
            int Id = Convert.ToInt32(userId);
              
            var response = await _memberService.GetProfileAsync(Id);
            if (response == null)
                return NotFound();

            return Ok(response);
        }

        [HttpGet("{Id}", Name = "GetMemeberById")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetMemberByIdResopnse>>
            GetMemeberById(int Id)
        {

            if (Id < 1)
                return BadRequest();

            var response = await _memberService.GetMemberByIdAsync(Id);
            if (response == null)
                return NotFound();

            return Ok(response);
        }

        [HttpPost(Name = "AddMember")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>> AddMember([FromBody] AddMemberRequest request)
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

                _=> CreatedAtRoute("GetMemeberById", result.NewId) 
            };
        }

        [HttpPut("{Id}",Name = "UpdateMember")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateMember(int Id,[FromBody]UpdateMemberRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var status = await _memberService.UpdateAsync(Id,request);
            return status switch
            {
                enUpdateMemberStatus.NotUniqueEmail => BadRequest("Email must be unique"),

                enUpdateMemberStatus.NotUniquePhone => BadRequest("Phone must be unique"),

                enUpdateMemberStatus.CoachInactive => BadRequest("Coach must be active"),

                enUpdateMemberStatus.DataNotChanged => BadRequest("Data not changed"),

                enUpdateMemberStatus.CoachNotExists => NotFound("Coach not found"),

                enUpdateMemberStatus.MemberNotFound => NotFound("Member not found"),

                _=> NoContent()
            };
        }

        [HttpPut("me", Name = "UpdateMemberProfile")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateMemberProfile([FromBody] UpdateMemberProfileRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized("Token is no longer valid");

            int Id = Convert.ToInt32(userId); 
            enUpdateMemberProfileStatus status = await _memberService
                .UpdateProfileAsync(Id,request);
            return status switch
            {
                enUpdateMemberProfileStatus.NotUniqueEmail => BadRequest("Email must be unique"),

                enUpdateMemberProfileStatus.NotUniquePhone => BadRequest("Phone must be unique"),

                enUpdateMemberProfileStatus.DataNotChanged => BadRequest("Data not changed"),

                enUpdateMemberProfileStatus.MemberNotFound => NotFound("Member not found"),
                 
                _=> NoContent()
            };
        }
    }
}
