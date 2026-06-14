
using RepositoryTier.Payment.DTOs;
using RepositoryTier.Payment.Repositories;
using RepositoryTier.Subscription.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RepositoryTier.Payment.Enums;
using System.Threading.Tasks;
using RepositoryTier.Entities;

namespace ServiceTier.Payment
{
    public class PaymentService : Service<RepositoryTier.Entities.Payment>, IPaymentService
    {
        private readonly IPaymentRepository _repo; 
        public PaymentService(
            IPaymentRepository repo,
            ISubscriptionRepository subscriptionRepo
            ) 
            : base(repo) 
        { 
            _repo = repo; 
        }

        public async Task<AddPaymentResponse> AddAsync(AddPaymentRequest request)
        {
            var summary = await _repo
                .GetSubscriptionPaymentSummaryAsync(request.SubscribtionId);

            if (!summary.Exists)
                return new AddPaymentResponse(enAddPaymentStatus.SubscriptionNotFound);

            if (summary.RemainingAmount <= 0)
                return new AddPaymentResponse(enAddPaymentStatus.SubscriptionFullPaid);

            if (summary.RemainingAmount < request.Amount)
                return new AddPaymentResponse(enAddPaymentStatus.PaidExceedsRemainingAmount);

            var payment = new RepositoryTier.Entities.Payment()
            {
                Amount=request.Amount,
                Notes=request.Notes,
                PaymentDate=DateOnly.FromDateTime(DateTime.UtcNow),
                SubscriptionId=request.SubscribtionId 
            };

            await _repo.AddAsync(payment);
            int affectedRows = await _repo.SaveChangesAsync();
            return new AddPaymentResponse(enAddPaymentStatus.Succeeded, payment.Id);
        }

       public async Task<GetPaymentByIdResponse?> GetByIdAsync(int Id)
        {
            return await _repo.GetByIdAsync(Id);
        }
    }
}
