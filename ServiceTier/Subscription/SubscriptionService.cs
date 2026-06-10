 
using RepositoryTier.Subscription.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace ServiceTier.Subscription
{
    public class SubscriptionService : Service<RepositoryTier.Entities.Subscription>, ISubscriptionService
    {
        public SubscriptionService(ISubscriptionRepository repo) : base(repo) { }

    }
}
