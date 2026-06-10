using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace RepositoryTier.Payment.Repositories
{
    public class PaymentRepository : Repository<Entities.Payment>, IPaymentRepository
    {
        public PaymentRepository(GymManagementDbContext context) : base(context) { }
    }
}
