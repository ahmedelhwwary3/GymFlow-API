using RepositoryTier.Subscription.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.Subscription.DTOs
{
    public class SubscriptionResponse
    {
        public int Id { get; set; }
        public string MemberName { get; set; } 
        public string CoachName {  get; set; } 
        public DateOnly StartDate { get; set; } 
        public DateOnly EndDate { get; set; }
        public enSubscriptonStatus Status { get; set; }
        public decimal Price { get; set; } 
    }
}
