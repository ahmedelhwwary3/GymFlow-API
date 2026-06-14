using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.Payment.DTOs
{
    public class AddPaymentRequest
    {
        [Range(1,int.MaxValue)]
        public int SubscribtionId { get; set; }

        [Range(1,int.MaxValue)]
        public decimal Amount { get; set; }
         
        public string? Notes { get; set; }
    }
}
