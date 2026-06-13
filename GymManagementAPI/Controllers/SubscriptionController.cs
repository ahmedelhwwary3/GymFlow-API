using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryTier.Subscription.DTOs;
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

        public async Task<ActionResult<GetSubscriptionsResponse>>GetSubscriptions(GetSubscriptionsRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var response = await _subscriptionService
                .GetSubscriptionsAsync(request);
            if (response == null || response.Count == 0)
                return NotFound("Subscriptions not found");

            return Ok(response);
        }




    }
}
