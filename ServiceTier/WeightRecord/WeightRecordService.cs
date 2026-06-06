using RepositoryTier.Data.Repositories;
using RepositoryTier.Data.Repositories.WeightRecord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using models = RepositoryTier.Models;

namespace ServiceTier.WeightRecord
{
    public class WeightRecordService : Service<models.WeightRecord>, IWeightRecordService
    {
        public WeightRecordService(IWeightRecordRepository repo) : base(repo) { } 
    }
}
