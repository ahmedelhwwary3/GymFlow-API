using RepositoryTier.Data.Repositories;
using RepositoryTier.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceTier.WorkoutPlanExercise
{
    public class WorkoutPlanExerciseService:Service<RepositoryTier.Models.WorkoutPlanExercise>,IWorkoutPlanExerciseService
    {
        public WorkoutPlanExerciseService(IRepository<RepositoryTier.Models.WorkoutPlanExercise>repo): base(repo) { }
    }
}
