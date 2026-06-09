 
using RepositoryTier.Payment.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using models = RepositoryTier.Models;

namespace ServiceTier.Payment
{
    public class PaymentService : Service<models.Payment>, IPaymentService
    {
        public PaymentService(IPaymentRepository repo) : base(repo) { }

    }
}
