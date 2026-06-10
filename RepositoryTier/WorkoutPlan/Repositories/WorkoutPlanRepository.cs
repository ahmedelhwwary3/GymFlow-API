using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace RepositoryTier.WorkoutPlan.Repositories
{
    public class WorkoutPlanRepository : Repository<Entities.WorkoutPlan>, IWorkoutPlanRepository
    {
        public WorkoutPlanRepository(GymManagementDbContext context) : base(context) { }
    }
}
