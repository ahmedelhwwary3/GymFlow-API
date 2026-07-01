
using RepositoryTier.WorkoutPlanExercise.Repositories;
using System;
using System.Threading.Tasks; 

namespace ServiceTier.WorkoutPlanExercise
{
    public class WorkoutPlanExerciseService:IWorkoutPlanExerciseService
    {
        private readonly IWorkoutPlanExerciseRepository _repo;
        public WorkoutPlanExerciseService(IWorkoutPlanExerciseRepository repo)
        { 
            _repo = repo;
        }
    }
}
