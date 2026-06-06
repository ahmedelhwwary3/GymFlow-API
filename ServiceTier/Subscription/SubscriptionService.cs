using RepositoryTier.Data.Repositories;
using RepositoryTier.Data.Repositories.Subscription;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceTier.Subscription
{
    public class SubscriptionService : Service<RepositoryTier.Models.Subscription>, ISubscriptionService
    {
        public SubscriptionService(ISubscriptionRepository repo) : base(repo) { }

    }
}
