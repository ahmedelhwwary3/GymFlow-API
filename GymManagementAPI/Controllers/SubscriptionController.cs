using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using RepositoryTier.Coach.DTOs;
using RepositoryTier.Subscription.DTOs;
using RepositoryTier.Subscription.Enums;
using RepositoryTier.Subscription.Results;
using ServiceTier.Subscription;

namespace GymManagementAPI.Controllers
{ 
    [Route("api/Subscription")]
    [ApiController]
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
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetSubscriptionsResponse>>
            GetSubscriptions([FromQuery] GetSubscriptionsRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var response = await _subscriptionService
                .GetSubscriptionsAsync(request);

            if (response == null || response.Count == 0)
                return NotFound("Subscriptions not found");

            return Ok(response);
        }

        [HttpPost(Name = "AddSubscription")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

                enAddSubscriptionStatus.HasActiveOrForzenSubscription => BadRequest("Member has an active Or forzen subscription"),
                
                enAddSubscriptionStatus.CoachInctive => BadRequest("Coach is not active"),

                enAddSubscriptionStatus.MemberInactive => BadRequest("Member is not active"),

                _=>CreatedAtRoute("GetSubscriptionById", response.Id)
            };
        }

        [HttpGet("{Id}", Name = "GetSubscriptionById")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<GetSubscriptionByIdResponse>> 
            GetSubscriptionById(int Id)
        {
            if (Id < 1)
                return BadRequest();

            var response = await _subscriptionService.GetByIdAsync(Id);
            return response == null ? NotFound("Subscription not found") : Ok(response);
        }

        [HttpPatch("{Id}/FreezeSubscription", Name = "FreezeSubscriptionById")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
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
