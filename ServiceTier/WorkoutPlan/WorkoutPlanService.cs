using RepositoryTier.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceTier.WorkoutPlan
{
    public class WorkoutPlanService: Service<RepositoryTier.Models.WorkoutPlan>, IWorkoutPlanService
    {
        public WorkoutPlanService(IRepository<RepositoryTier.Models.WorkoutPlan>repo) : base(repo) { }
    }
}
