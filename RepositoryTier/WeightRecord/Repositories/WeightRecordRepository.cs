using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace RepositoryTier.WeightRecord.Repositories
{
    public class WeightRecordRepository : Repository<Entities.WeightRecord>, IWeightRecordRepository
    {
        public WeightRecordRepository(GymManagementDbContext context) : base(context) { }
    }
}
