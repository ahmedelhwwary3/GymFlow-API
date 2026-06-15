 
using RepositoryTier.Subscription.DTOs;
using RepositoryTier.Subscription.Enums;
using RepositoryTier.Subscription.Results;
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
        Task<AddSubscriptionResult> AddAsync(AddSubscriptionRequest request);
        Task<enFreezeSubscriptionStatus> FreezeSubscriptionAsync(int Id, FreezeSubscriptionByIdRequest request);
        Task<GetSubscriptionByIdResponse?> GetByIdAsync(int Id); 
    }
}
