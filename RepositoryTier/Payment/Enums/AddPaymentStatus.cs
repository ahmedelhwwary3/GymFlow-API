using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.Payment.Enums
{
    public enum enAddPaymentStatus
    {
        SubscriptionNotFound=1,
        SubscriptionFullPaid =2,
        PaidExceedsRemainingAmount=3,
        Succeeded=4
    }
}
