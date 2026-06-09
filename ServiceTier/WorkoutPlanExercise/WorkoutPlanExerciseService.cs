using RepositoryTier.Data.Repositories;
using RepositoryTier.WorkoutPlanExercise.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using models = RepositoryTier.Models;
namespace ServiceTier.WorkoutPlanExercise
{
    public class WorkoutPlanExerciseService:Service<models.WorkoutPlanExercise>,IWorkoutPlanExerciseService
    {
        public WorkoutPlanExerciseService(IWorkoutPlanExerciseRepository repo): base(repo) { }
    }
}
