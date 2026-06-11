 
using RepositoryTier.WeightRecord.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace ServiceTier.WeightRecord
{
    public class WeightRecordService : Service<RepositoryTier.Entities.WeightRecord>, IWeightRecordService
    {
        private readonly IWeightRecordRepository _repo;
        public WeightRecordService(IWeightRecordRepository repo) : base(repo) 
        {
            _repo = repo;
        } 
    }
}
