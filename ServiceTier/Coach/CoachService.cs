using RepositoryTier.Data.Repositories;
using RepositoryTier.Data.Repositories.Attendance;
using RepositoryTier.Data.Repositories.Coach;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceTier.Coach
{
    public class CoachService : Service<RepositoryTier.Models.Coach>, ICoachService
    {
        public CoachService(ICoachRepository repo) : base(repo) { }
    }
}
