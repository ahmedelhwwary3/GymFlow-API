using Azure.Core;
using GymManagementAPI.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RepositoryTier.Coach.DTOs;
using RepositoryTier.Subscription.DTOs;
using RepositoryTier.Subscription.Enums;
using RepositoryTier.Subscription.Results;
using RepositoryTier.User.Enums;
using ServiceTier.Subscription;

namespace GymManagementAPI.Controllers
{ 
    [Route("api/Subscription")]
    [ApiController]
    [Authorize]
    public class SubscriptionController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;
        public SubscriptionController(ISubscriptionService subscriptionService)
        {
            _subscriptionService = subscriptionService;
        }
         
        [HttpGet("Subscriptions", Name = "GetSubscriptions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GetSubscriptionsResponse>>
            GetSubscriptions([FromQuery] GetSubscriptionsRequest request,
            [FromServices]IAuthorizationService authService)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            if (request.MemberId.HasValue)
            {
                var authResult = await authService
                    .AuthorizeAsync(User,request.MemberId,Policies.OwnerOrAdmin);

                if (!authResult.Succeeded)
                    return Forbid();
            }

            bool isAdmin = User.IsInRole($"{(int)enUserRole.Admin}");
            if (!isAdmin)
                return Forbid();

            var response = await _subscriptionService
                .GetSubscriptionsAsync(request); 

            return Ok(response);
        }

        [Authorize(Roles =UserRoles.Admin)]
        [HttpPost(Name = "AddSubscription")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<AddSubscriptionResult>>
            AddSubscription([FromBody] AddSubscriptionRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var response = await _subscriptionService.AddAsync(request);

            return response.Status switch
            {
                enAddSubscriptionStatus.MemberNotFound => NotFound("Member not found"),

                enAddSubscriptionStatus.CoachNotFound => NotFound("Coach not found"),

                enAddSubscriptionStatus.MemberNotAttachedToCoach => BadRequest("Member not attached to coach"),

                enAddSubscriptionStatus.HasActiveSubscription => BadRequest("Member has an active subscription"),

                enAddSubscriptionStatus.HasForzenSubscription => BadRequest("Member has a frozen subscription"),

                enAddSubscriptionStatus.CoachInctive => BadRequest("Coach is not active"),

                enAddSubscriptionStatus.MemberInactive => Unauthorized("Member is not active"),

                _=>CreatedAtRoute("GetSubscriptionById",new { Id=response.Id},null)
            };
        }

        [HttpGet("{Id}", Name = "GetSubscriptionById")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GetSubscriptionByIdResponse>> 
            GetSubscriptionById(int Id, [FromServices] IAuthorizationService authService)
        {
            if (Id < 1)
                return BadRequest();

            var authResult = await authService
                    .AuthorizeAsync(User, Id, Policies.SubscriptionOwnerOrAdmin);

            if (!authResult.Succeeded)
                return Forbid();

            var response = await _subscriptionService.GetByIdAsync(Id);
            return response == null ? NotFound("Subscription is not found") : Ok(response);
        }

        [Authorize(Roles =$"{UserRoles.Admin}")]
        [HttpPatch("{Id}/FreezeSubscription", Name = "FreezeSubscriptionById")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult>
            FreezeSubscriptionById(int Id,FreezeSubscriptionByIdRequest request)
        {
            if (Id < 1 || !ModelState.IsValid)
                return BadRequest();

            enFreezeSubscriptionStatus status = await _subscriptionService
                .FreezeSubscriptionAsync(Id, request);

            return status switch
            {
                enFreezeSubscriptionStatus.SubscriptionExpired => BadRequest("Subscription is expired"),

                enFreezeSubscriptionStatus.SubscriptionAlreadyFrozen => BadRequest("Subscription is already frozen"),

                enFreezeSubscriptionStatus.SubscriptionNotFound => NotFound("Subscription not found"),

                _ => NoContent()
            }; 
        }
    }
}
