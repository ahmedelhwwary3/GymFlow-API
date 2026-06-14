using RepositoryTier.Payment.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace RepositoryTier.Payment.Repositories
{
    public interface IPaymentRepository:IRepository<Entities.Payment>
    {
        Task<SubscriptionPaymentSummaryResponse> GetSubscriptionPaymentSummaryAsync(int subscriptionId);
    }
}
