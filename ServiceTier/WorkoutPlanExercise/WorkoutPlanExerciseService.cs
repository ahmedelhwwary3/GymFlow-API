
using RepositoryTier.WorkoutPlanExercise.Repositories;
using System;
using System.Threading.Tasks; 

namespace ServiceTier.WorkoutPlanExercise
{
    public class WorkoutPlanExerciseService:Service
        <RepositoryTier.Entities.WorkoutPlanExercise>,IWorkoutPlanExerciseService
    {
        public WorkoutPlanExerciseService(IWorkoutPlanExerciseRepository repo): base(repo) { }
    }
}
