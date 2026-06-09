 
using RepositoryTier.Subscription.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using models = RepositoryTier.Models;

namespace ServiceTier.Subscription
{
    public class SubscriptionService : Service<models.Subscription>, ISubscriptionService
    {
        public SubscriptionService(ISubscriptionRepository repo) : base(repo) { }

    }
}
