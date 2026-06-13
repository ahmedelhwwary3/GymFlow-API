using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.Subscription.Enums
{
    public enum enFreezeSubscriptionStatus
    {
        SubscriptionNotFound=1, 
        SubscriptionExpired = 2,
        SubscriptionAlreadyFrozen=3,
        Succeeded =4
    }
}
