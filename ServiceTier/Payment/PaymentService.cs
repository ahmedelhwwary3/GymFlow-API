 
using RepositoryTier.Payment.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace ServiceTier.Payment
{
    public class PaymentService : Service<RepositoryTier.Entities.Payment>, IPaymentService
    {
        public PaymentService(IPaymentRepository repo) : base(repo) { }

    }
}
