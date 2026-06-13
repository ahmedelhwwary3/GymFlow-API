using RepositoryTier.Coach.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.Subscription.DTOs
{
    public class GetSubscriptionsResponse
    {
        public List<SubscriptionResponse> Subscriptions { get; set; }
        public List<CoachLookUpResponse> Coaches { get; set; }
        public int Count { get; set; }
    }
}
