using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.Data.Repositories.WeightRecord
{
    internal class WeightRecordRepository : Repository<GymManagementAPI.Models.WeightRecord>, IWeightRecordRepository
    {
        public WeightRecordRepository(GymManagementDbContext context) : base(context) { }
    }
}
