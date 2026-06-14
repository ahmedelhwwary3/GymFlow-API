using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.Payment.DTOs
{
    public class SubscriptionPaymentSummaryResponse
    {
        public bool Exists { get; set; }
        public decimal? RemainingAmount { get; set; } 

        public SubscriptionPaymentSummaryResponse(bool exists,
            decimal? remainingAmount = null )
        { 
            this.Exists = exists;
            this.RemainingAmount= remainingAmount;
        }
    }
}
