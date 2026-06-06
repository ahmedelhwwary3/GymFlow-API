using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.Data.Repositories.WorkoutPlan
{
    internal class WorkoutPlanRepository : Repository<GymManagementAPI.Models.WorkoutPlan>, IWorkoutPlanRepository
    {
        public WorkoutPlanRepository(GymManagementDbContext context) : base(context) { }
    }
}
