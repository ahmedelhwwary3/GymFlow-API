using GymManagementAPI.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using RepositoryTier.Payment.DTOs;
using RepositoryTier.Payment.Enums;
using ServiceTier.Payment;

namespace GymManagementAPI.Controllers
{
    [Route("api/Payment")]
    [ApiController]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        public PaymentController(IPaymentService paymentService)
        {
            _paymentService= paymentService;
        }

        [EnableRateLimiting(Policies.SlidingWindowAuthLimiter)]
        [Authorize(Roles =UserRoles.Admin)]
        [HttpPost(Name = "AddPayment")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<int>> AddPayment(AddPaymentRequest request) 
        {
            if(!ModelState.IsValid)
                return BadRequest();
            
            var response = await _paymentService.AddAsync(request);
            return response.Status switch
            {
                enAddPaymentStatus.SubscriptionNotFound => NotFound("Subscription not found"),

                enAddPaymentStatus.SubscriptionFullPaid => BadRequest("Subscription is full paid"),

                enAddPaymentStatus.PaidExceedsRemainingAmount => BadRequest("Paid amount exceeds remaining amount"),

                _=> CreatedAtRoute("GetPaymentById", new { Id= response.Id },null) 
            };
        }

        [EnableRateLimiting(Policies.SlidingWindowAuthLimiter)]
        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet("{Id}",Name = "GetPaymentById")] 
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<GetPaymentByIdResponse>> GetPaymentById(int Id)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var response = await _paymentService.GetByIdAsync(Id);

            return response == null ? NotFound("Payment not found") : Ok(response);
        }
    }
}
