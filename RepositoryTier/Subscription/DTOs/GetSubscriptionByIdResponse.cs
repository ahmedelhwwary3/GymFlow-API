using RepositoryTier.Subscription.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.Subscription.DTOs
{
    public class GetSubscriptionByIdResponse
    {
        public int Id { get; set; }
        public int MemberId { get; set; }
        public string MemberName { get; set; }
        public string MemberPhone { get; set; }
        public int CoachId { get; set; }
        public string CoachName { get; set; }
        public enSubscriptionPlan Plan { get; set; }
        public decimal SubscriptionPrice { get; set; }
        public decimal TotalPaid { get; set; }
        public decimal RemainingAmount => SubscriptionPrice - TotalPaid;
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public enSubscriptonStatus Status { get; set; }

        public GetSubscriptionByIdResponse()
        {
            if (RemainingAmount < 0)
                throw new Exception("Remaining amount must be greater than or equal to 0");
        }

    }
}
