 
using RepositoryTier.WeightRecord.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace ServiceTier.WeightRecord
{
    public class WeightRecordService :  IWeightRecordService
    {
        private readonly IWeightRecordRepository _repo;
        public WeightRecordService(IWeightRecordRepository repo)  
        {
            _repo = repo;
        } 
    }
}
