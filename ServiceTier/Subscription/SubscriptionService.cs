using RepositoryTier.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceTier.Subscription
{
    public class SubscriptionService : Service<RepositoryTier.Models.Subscription>, ISubscriptionService
    {
        public SubscriptionService(IRepository<RepositoryTier.Models.Subscription> repo) : base(repo) { }

    }
}
