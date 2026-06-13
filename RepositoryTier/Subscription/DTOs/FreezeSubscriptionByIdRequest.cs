using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RepositoryTier.CustomAttributes;
using System.Threading.Tasks;

namespace RepositoryTier.Subscription.DTOs
{
    public class FreezeSubscriptionByIdRequest
    {
        [NotPastDateOnly]
        [DateLessThan(nameof(FreezeEndDate))]  
        public DateOnly FreezeStartDate { get; set; } 
        public DateOnly FreezeEndDate { get; set; }
    }
}
