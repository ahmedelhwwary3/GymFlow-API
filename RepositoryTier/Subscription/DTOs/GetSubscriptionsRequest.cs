using RepositoryTier.Coach.Enums;
using RepositoryTier.Subscription.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.Subscription.DTOs
{
    public class GetSubscriptionsRequest
    {
        [Range(1,int.MaxValue)]
        public int? MemberId { get; set; }
        public string? Search { get; set; }

        [EnumDataType(typeof(enSubscriptonStatus))]
        public enSubscriptonStatus? Status { get; set; } = null;

        [EnumDataType(typeof(enSubscriptionPlan))]
        public enSubscriptionPlan? Plan { get; set; } = null;
         
        [EnumDataType(typeof(enSubscriptonSort))]
        public enSubscriptonSort? Sort { get; set; }

        [Range(1,int.MaxValue)]
        public int? Page { get; set; }

        [Range(1, int.MaxValue)]
        public int? PageSize { get; set; } 
    }
}
