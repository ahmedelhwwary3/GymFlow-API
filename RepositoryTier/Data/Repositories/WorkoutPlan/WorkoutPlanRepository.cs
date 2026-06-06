using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.Data.Repositories.WorkoutPlan
{
    public class WorkoutPlanRepository : Repository<RepositoryTier.Models.WorkoutPlan>, IWorkoutPlanRepository
    {
        public WorkoutPlanRepository(GymManagementDbContext context) : base(context) { }
    }
}
