using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.Data.Repositories.Payment
{
    public class PaymentRepository : Repository<RepositoryTier.Models.Payment>, IPaymentRepository
    {
        public PaymentRepository(GymManagementDbContext context) : base(context) { }
    }
}
