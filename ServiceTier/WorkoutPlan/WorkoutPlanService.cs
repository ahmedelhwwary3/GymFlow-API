 
using RepositoryTier.WorkoutPlan.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace ServiceTier.WorkoutPlan
{
    public class WorkoutPlanService: Service<RepositoryTier.Entities.WorkoutPlan>, IWorkoutPlanService
    {
        private readonly IWorkoutPlanRepository _repo;
        public WorkoutPlanService(IWorkoutPlanRepository repo) : base(repo) 
        {
            _repo = repo;
        }
    }
}
