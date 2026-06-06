using ServiceTier.WorkoutPlanExercise;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace ServiceTier
{
    public interface IWorkoutPlanExerciseService:IService<RepositoryTier.Models.WorkoutPlanExercise>
    {
    }
}
