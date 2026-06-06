using RepositoryTier.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceTier.WeightRecord
{
    public class WeightRecordService : Service<RepositoryTier.Models.WeightRecord>, IWeightRecordService
    {
        public WeightRecordService(IRepository<RepositoryTier.Models.WeightRecord> repo) : base(repo) { } 
    }
}
