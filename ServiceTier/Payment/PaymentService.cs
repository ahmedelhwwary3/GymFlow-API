using RepositoryTier.Data.Repositories;
using RepositoryTier.Data.Repositories.Payment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceTier.Payment
{
    public class PaymentService : Service<RepositoryTier.Models.Payment>, IPaymentService
    {
        public PaymentService(IPaymentRepository repo) : base(repo) { }

    }
}
