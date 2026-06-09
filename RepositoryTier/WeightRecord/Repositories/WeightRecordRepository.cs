using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using models = RepositoryTier.Models;

namespace RepositoryTier.WeightRecord.Repositories
{
    public class WeightRecordRepository : Repository<models.WeightRecord>, IWeightRecordRepository
    {
        public WeightRecordRepository(GymManagementDbContext context) : base(context) { }
    }
}
