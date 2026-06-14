using Microsoft.EntityFrameworkCore;
using RepositoryTier.Payment.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace RepositoryTier.Payment.Repositories
{
    public class PaymentRepository : Repository<Entities.Payment>, IPaymentRepository
    {
        public PaymentRepository(GymManagementDbContext context) : base(context) { }

        public async Task<SubscriptionPaymentSummaryResponse> 
            GetSubscriptionPaymentSummaryAsync(int subscriptionId)
        {
            bool subscriptionExists = await _context.Subscriptions 
                .AnyAsync(s => s.Id == subscriptionId);

            if (!subscriptionExists)
                return new SubscriptionPaymentSummaryResponse(false);

            decimal remainingAmount = await _context.Subscriptions
            .Where(s => s.Id == subscriptionId)
            .Select(s => new
            {
                RemainingAmount = s.Price -
                    (_context.Payments
                        .Where(p => p.SubscriptionId == s.Id)
                        .Sum(p => p.Amount))
            })
            .Select(x => x.RemainingAmount)
            .FirstOrDefaultAsync();

            return new SubscriptionPaymentSummaryResponse(true, remainingAmount);
        }
    }
}
