using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 

namespace RepositoryTier.Subscription.Repositories
{
    public class SubscriptionRepository : Repository<Entities.Subscription>, ISubscriptionRepository
    {
        public SubscriptionRepository(GymManagementDbContext context) : base(context) { }

    }
}
