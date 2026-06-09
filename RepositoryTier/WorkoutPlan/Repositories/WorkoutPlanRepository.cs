using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using models = RepositoryTier.Models;

namespace RepositoryTier.WorkoutPlan.Repositories
{
    public class WorkoutPlanRepository : Repository<models.WorkoutPlan>, IWorkoutPlanRepository
    {
        public WorkoutPlanRepository(GymManagementDbContext context) : base(context) { }
    }
}
