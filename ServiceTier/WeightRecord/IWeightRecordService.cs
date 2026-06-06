using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceTier
{
    public interface IWeightRecordService : IService<RepositoryTier.Models.WeightRecord>
    {
    }
}
