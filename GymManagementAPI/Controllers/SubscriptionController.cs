using Azure;
using Azure.Core;
using GymManagementAPI.Extensions;
using GymManagementAPI.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using RepositoryTier.Coach.DTOs;
using RepositoryTier.Subscription.DTOs;
using RepositoryTier.Subscription.Enums;
using RepositoryTier.Subscription.Results;
using RepositoryTier.User.Enums;
using ServiceTier.Subscription;
using log = GymManagementAPI.Extensions.IloggerExtensions;

namespace GymManagementAPI.Controllers
{ 
    [Route("api/Subscription")]
    [ApiController]
    [Authorize]
    public class SubscriptionController : ControllerBase
    {
        private readonly ISubscriptionService _subscriptionService;
        private readonly ILogger _logger;
        private string _Ip;
        private string _adminId;
        public SubscriptionController(ISubscriptionService subscriptionService,ILogger logger)
        {
            _subscriptionService = subscriptionService;
            _logger = logger;
            _adminId = HttpContext.GetAdminId();
            _Ip = HttpContext.GetIPAddress();
        }

        [EnableRateLimiting(Policies.TokenBucketAuthLimiter)]
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
            {
                _logger.LogAdminInvalidAction(nameof(GetSubscriptions),
                    log.enInvalidAdminActionReason.InvalidInput, _adminId, _Ip);
                return BadRequest();
            }
              
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

        [EnableRateLimiting(Policies.SlidingWindowAuthLimiter)]
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
            {
                _logger.LogAdminInvalidAction(nameof(AddSubscription),
                    log.enInvalidAdminActionReason.InvalidInput, _adminId, _Ip);
                return BadRequest();
            }

            var response = await _subscriptionService.AddAsync(request);

            if (response.Status == enAddSubscriptionStatus.Succeeded)
            {
                _logger.LogAdminExecutedAction(nameof(AddSubscription),_adminId,_Ip);
                return CreatedAtRoute("GetSubscriptionById", new { Id = response.Id }, null);
            }

            _logger.LogAdminInvalidAction(nameof(AddSubscription),
                log.enInvalidAdminActionReason.LogicalError, _adminId, _Ip);

            return response.Status switch
            {
                enAddSubscriptionStatus.MemberNotFound => NotFound("Member not found"),

                enAddSubscriptionStatus.CoachNotFound => NotFound("Coach not found"),

                enAddSubscriptionStatus.MemberNotAttachedToCoach => BadRequest("Member not attached to coach"),

                enAddSubscriptionStatus.HasActiveSubscription => BadRequest("Member has an active subscription"),

                enAddSubscriptionStatus.HasForzenSubscription => BadRequest("Member has a frozen subscription"),

                enAddSubscriptionStatus.CoachInctive => BadRequest("Coach is not active"),

                enAddSubscriptionStatus.MemberInactive => Unauthorized("Member is not active"),

                _=> StatusCode(StatusCodes.Status500InternalServerError)
            };
        }

        [EnableRateLimiting(Policies.SlidingWindowAuthLimiter)]
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
            {
                _logger.LogAdminInvalidAction(nameof(AddSubscription),
                    log.enInvalidAdminActionReason.InvalidInput, _adminId, _Ip);
                return BadRequest();
            }
           
            var authResult = await authService
                    .AuthorizeAsync(User, Id, Policies.SubscriptionOwnerOrAdmin);

            if (!authResult.Succeeded)
                return Forbid();

            var response = await _subscriptionService.GetByIdAsync(Id);
            return response == null ? NotFound("Subscription is not found") : Ok(response);
        }

        [EnableRateLimiting(Policies.SlidingWindowAuthLimiter)]
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
            {

                return BadRequest();
            }

            enFreezeSubscriptionStatus status = await _subscriptionService
                .FreezeSubscriptionAsync(Id, request);

            if (status == enFreezeSubscriptionStatus.Succeeded)
            {
                _logger.LogAdminExecutedAction(nameof(FreezeSubscriptionById), _adminId, _Ip);
                return NoContent();
            }

            _logger.LogAdminInvalidAction(nameof(FreezeSubscriptionById),
                log.enInvalidAdminActionReason.LogicalError, _adminId, _Ip);

            return status switch
            {
                enFreezeSubscriptionStatus.SubscriptionExpired => BadRequest("Subscription is expired"),

                enFreezeSubscriptionStatus.SubscriptionAlreadyFrozen => BadRequest("Subscription is already frozen"),

                enFreezeSubscriptionStatus.SubscriptionNotFound => NotFound("Subscription not found"),

                _ => StatusCode(StatusCodes.Status500InternalServerError)
            }; 
        }
    }
}
