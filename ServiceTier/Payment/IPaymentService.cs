using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using models = RepositoryTier.Models;

namespace ServiceTier.Payment
{
    public interface IPaymentService:IService<models.Payment>
    {
    }
}
