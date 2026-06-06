using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.Data.Repositories.Coach
{
    internal class CoachRepository : Repository<GymManagementAPI.Models.Coach>, ICoachRepository
    {
        public CoachRepository(GymManagementDbContext context) : base(context) { }

    }
}
