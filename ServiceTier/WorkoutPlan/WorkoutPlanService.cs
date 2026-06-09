 
using RepositoryTier.WorkoutPlan.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using models = RepositoryTier.Models;

namespace ServiceTier.WorkoutPlan
{
    public class WorkoutPlanService: Service<models.WorkoutPlan>, IWorkoutPlanService
    {
        public WorkoutPlanService(IWorkoutPlanRepository repo) : base(repo) { }
    }
}
