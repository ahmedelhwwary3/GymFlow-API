using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.Data.Repositories.Subscription
{
    public class SubscriptionRepository : Repository<RepositoryTier.Models.Subscription>, ISubscriptionRepository
    {
        public SubscriptionRepository(GymManagementDbContext context) : base(context) { }

    }
}
