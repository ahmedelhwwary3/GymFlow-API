using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.Data.Repositories.Payment
{
    internal class PaymentRepository : Repository<GymManagementAPI.Models.Payment>, IPaymentRepository
    {
        public PaymentRepository(GymManagementDbContext context) : base(context) { }
    }
}
