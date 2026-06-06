using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.Data.Repositories.Subscription
{
    internal class SubscriptionRepository : Repository<GymManagementAPI.Models.Subscription>, ISubscriptionRepository
    {
        public SubscriptionRepository(GymManagementDbContext context) : base(context) { }

    }
}
