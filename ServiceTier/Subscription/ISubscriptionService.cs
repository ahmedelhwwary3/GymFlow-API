using RepositoryTier.Subscription.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace ServiceTier.Subscription
{
    public interface ISubscriptionService:IService<RepositoryTier.Entities.Subscription>
    {
        Task<GetSubscriptionsResponse> GetSubscriptionsAsync(GetSubscriptionsRequest request);
    }
}
