using RepositoryTier.Subscription.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepositoryTier.CustomAttributes;

namespace RepositoryTier.Subscription.DTOs
{
    public class AddSubscriptionRequest
    {
        [Range(1,int.MaxValue)]
        public int MemberId { get; set; }

        [EnumDataType(typeof(enSubscriptionPlan))]
        public enSubscriptionPlan SubscriptionPlan { get; set; }

        [Range(1,int.MaxValue)]
        public decimal Price { get; set; }

        [NotPastDateOnlyAttribute]
        public DateOnly StartDate { get; set; }    
    }
}
