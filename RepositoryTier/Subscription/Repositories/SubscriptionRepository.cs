using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using models = RepositoryTier.Models;

namespace RepositoryTier.Subscription.Repositories
{
    public class SubscriptionRepository : Repository<models.Subscription>, ISubscriptionRepository
    {
        public SubscriptionRepository(GymManagementDbContext context) : base(context) { }

    }
}
