
using RepositoryTier.WorkoutPlanExercise.Repositories;
using System;
using System.Threading.Tasks; 

namespace ServiceTier.WorkoutPlanExercise
{
    public class WorkoutPlanExerciseService:Service
        <RepositoryTier.Entities.WorkoutPlanExercise>,IWorkoutPlanExerciseService
    {
        private readonly IWorkoutPlanExerciseRepository _repo;
        public WorkoutPlanExerciseService(IWorkoutPlanExerciseRepository repo): base(repo) 
        { 
            _repo = repo;
        }
    }
}
