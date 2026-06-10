using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace ServiceTier.Payment
{
    public interface IPaymentService:IService<RepositoryTier.Entities.Payment>
    {
    }
}
