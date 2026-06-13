using RepositoryTier.Subscription.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.Subscription.DTOs
{
    public class AddSubscriptionRequest
    {
        public int MemberId { get; set; } 
        public int CoachId { get; set; } 
        public enSubscriptionPlan SubscriptionPlan { get; set; } 
        public decimal Price { get; set; } 
        public DateOnly StartDate { get; set; } 
        public DateOnly EndDate { get; set; }     
    }
}
