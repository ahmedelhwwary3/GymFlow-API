using RepositoryTier.Coach.Enums;
using RepositoryTier.Subscription.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.Subscription.Results
{
    public class AddSubscriptionResult
    {
        public int? Id { get; set; }
        public enAddSubscriptionStatus Status { get; set; }

        public AddSubscriptionResult(enAddSubscriptionStatus status,int? Id=null)
        {
            this.Id=Id;
            this.Status=status;
        }
    }
}
