using GymManagementAPI.Extensions;
using GymManagementAPI.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using RepositoryTier.Payment.DTOs;
using RepositoryTier.Payment.Enums;
using ServiceTier.Payment;
using log = GymManagementAPI.Extensions.IloggerExtensions;

namespace GymManagementAPI.Controllers
{
    [Route("api/Payment")]
    [ApiController]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger _logger;
        private string _Ip;
        private string _adminId;
        public PaymentController(IPaymentService paymentService,ILogger logger)
        {
            _paymentService= paymentService;
            _logger= logger;
            _adminId = HttpContext.GetAdminId();
            _Ip = HttpContext.GetIPAddress();
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
            if (!ModelState.IsValid)
            {
                _logger.LogAdminInvalidAction(nameof(AddPayment),
                 log.enInvalidAdminActionReason.InvalidInput, _adminId, _Ip);
                return BadRequest();
            }
                 
            var response = await _paymentService.AddAsync(request);
            if(response.Status==enAddPaymentStatus.Succeeded)
            {
                _logger.LogAdminExecutedAction(nameof(AddPayment), _adminId, _Ip);
                return CreatedAtRoute("GetPaymentById", new { Id = response.Id }, null);
            }

            _logger.LogAdminInvalidAction(nameof(AddPayment),
            log.enInvalidAdminActionReason.LogicalError, _adminId, _Ip);
            return response.Status switch
            {
                enAddPaymentStatus.SubscriptionNotFound => NotFound("Subscription not found"),

                enAddPaymentStatus.SubscriptionFullPaid => BadRequest("Subscription is full paid"),

                enAddPaymentStatus.PaidExceedsRemainingAmount => BadRequest("Paid amount exceeds remaining amount"),

                _=> StatusCode(StatusCodes.Status500InternalServerError)
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
            {
                _logger.LogAdminInvalidAction(nameof(AddPayment),
                 log.enInvalidAdminActionReason.InvalidInput, _adminId, _Ip);
                return BadRequest();
            }
         
            var response = await _paymentService.GetByIdAsync(Id);

            return response == null ? NotFound("Payment not found") : Ok(response);
        }
    }
}
