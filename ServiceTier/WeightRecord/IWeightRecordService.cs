using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using models = RepositoryTier.Models;

namespace ServiceTier.WeightRecord
{
    public interface IWeightRecordService : IService<models.WeightRecord>
    {
    }
}
