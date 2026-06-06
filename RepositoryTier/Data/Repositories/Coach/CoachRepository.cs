using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.Data.Repositories.Coach
{
    public class CoachRepository : Repository<RepositoryTier.Models.Coach>, ICoachRepository
    {
        public CoachRepository(GymManagementDbContext context) : base(context) { }

    }
}
