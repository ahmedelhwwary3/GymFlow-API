using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.Payment.DTOs
{
    public class GetPaymentByIdResponse
    {
        public int Id { get; set; }

        public decimal Amount { get; set; }

        public DateOnly PaymentDate { get; set; }

        public string? Notes { get; set; }

        public int SubscriptionId { get; set; }

        public string MemberName { get; set; } = null!;

        public decimal SubscriptionPrice { get; set; }

        public decimal TotalPaid { get; set; }

        public decimal RemainingAmount => SubscriptionPrice - TotalPaid;

    }
}
