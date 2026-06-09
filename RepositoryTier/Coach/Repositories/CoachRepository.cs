using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using models = RepositoryTier.Models;

namespace RepositoryTier.Coach.Repositories
{
    public class CoachRepository : Repository<models.Coach>, ICoachRepository
    {
        public CoachRepository(GymManagementDbContext context) : base(context) { }

    }
}
