using GymManagementAPI.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using RepositoryTier.Coach.Enums;
using RepositoryTier.Member.DTOs;
using RepositoryTier.Member.Enums;
using RepositoryTier.User.Enums;
using ServiceTier.Member;
using System.Security.Claims;

namespace GymManagementAPI.Controllers
{
    [Route("api/Member")]
    [ApiController]
    [Authorize]
    public class MemberController : ControllerBase
    {
        private readonly IMemberService _memberService;
        public MemberController(IMemberService memberService)
        {
            _memberService = memberService;
        }

        [EnableRateLimiting(Policies.TokenBucketAuthLimiter)]
        [HttpGet("AssignedMembersForCoach", Name = "GetAssignedMembersForCoach")] 
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<GetAssignedMembersForCoachResponse>>
            GetAssignedMembersForCoach(int coachId,[FromQuery] GetAssignedMembersForCoachRequest request,
            [FromServices]IAuthorizationService authService)
        {
            if(!ModelState.IsValid || coachId<1)
                return BadRequest();

            var authResult = await authService
                .AuthorizeAsync(User,coachId,Policies.OwnerOrAdmin);

            if (!authResult.Succeeded)
                return Forbid();

            var response = await _memberService
                .GetAssignedMembersForCoachAsync(coachId,request); 

            return Ok(response);
        }

        [EnableRateLimiting(Policies.TokenBucketAuthLimiter)]
        [Authorize(Roles =UserRoles.Admin)]
        [HttpGet("Members", Name = "GetMembers")] 
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GetMembersResopnse>>
            GetMembers([FromQuery] GetMembersRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var response = await _memberService.GetMembersAsync(request);
             
            return Ok(response);
        }

        [EnableRateLimiting(Policies.SlidingWindowAuthLimiter)]
        [HttpGet("profile",Name = "GetMemeberProfileById")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GetMemberByIdResopnse>>
            GetMemeberProfileById(int Id, [FromServices]IAuthorizationService authService)
        {
            var authResult = await authService
                .AuthorizeAsync(User,Id,Policies.OwnerOrAdmin);

            if (!authResult.Succeeded)
                return Forbid();

            var response = await _memberService.GetProfileByIdAsync(Id);
            if (response == null)
                return NotFound();

            return Ok(response);
        }

        [EnableRateLimiting(Policies.SlidingWindowAuthLimiter)]
        [HttpGet("{Id}", Name = "GetMemeberById")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GetMemberByIdResopnse>>
            GetMemeberById(int Id, [FromServices]IAuthorizationService authService)
        { 
            if (Id < 1)
                return BadRequest();

            var authResult = await authService
                .AuthorizeAsync(User,Id,Policies.OwnerOrAdmin);

            if (!authResult.Succeeded)
                return Forbid();

            var response = await _memberService.GetByIdAsync(Id);
            if (response == null)
                return NotFound();

            return Ok(response);
        }

        [EnableRateLimiting(Policies.SlidingWindowAuthLimiter)]
        [Authorize(Roles =$"{UserRoles.Admin}")]
        [HttpPut("{Id}",Name = "UpdateMember")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateMember(int Id,[FromBody]UpdateMemberRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var status = await _memberService.UpdateAsync(Id,request);
            return status switch
            {
                enUpdateMemberStatus.NotUniqueEmail => Conflict("Email must be unique"),

                enUpdateMemberStatus.NotUniquePhone => Conflict("Phone must be unique"),

                enUpdateMemberStatus.CoachInactive => Unauthorized("Coach must be active"),

                enUpdateMemberStatus.DataNotChanged => BadRequest("Data not changed"),

                enUpdateMemberStatus.CoachNotExists => NotFound("Coach not found"),

                enUpdateMemberStatus.MemberNotFound => NotFound("Member not found"),

                _=> NoContent()
            };
        }

        [EnableRateLimiting(Policies.SlidingWindowAuthLimiter)]
        [HttpPut("profile", Name = "UpdateMemberProfile")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateMemberProfile(int Id,[FromBody] UpdateMemberProfileRequest request, 
            [FromServices] IAuthorizationService authService)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var authResult = await authService
               .AuthorizeAsync(User, Id, Policies.OwnerOrAdmin);

            if (!authResult.Succeeded)
                return Forbid();

            enUpdateMemberProfileStatus status = await _memberService
                .UpdateProfileAsync(Id,request);

            return status switch
            {
                enUpdateMemberProfileStatus.NotUniqueEmail => Conflict("Email must be unique"),

                enUpdateMemberProfileStatus.NotUniquePhone => Conflict("Phone must be unique"),

                enUpdateMemberProfileStatus.DataNotChanged => BadRequest("Data not changed"),

                enUpdateMemberProfileStatus.MemberNotFound => NotFound("Member not found"),
                 
                _=> NoContent()
            };
        }
    }
}
