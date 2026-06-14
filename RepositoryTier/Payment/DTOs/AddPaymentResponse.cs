using RepositoryTier.Payment.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.Payment.DTOs
{
    public class AddPaymentResponse
    {
        public int? Id { get; set; }
        public enAddPaymentStatus Status { get; set; }

        public AddPaymentResponse(enAddPaymentStatus status,int? Id=null)
        {
            this.Status= status;
            this.Id= Id;
        }
    }
}
