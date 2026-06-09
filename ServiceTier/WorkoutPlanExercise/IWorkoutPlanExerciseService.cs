using ServiceTier.WorkoutPlanExercise;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using models = RepositoryTier.Models;

namespace ServiceTier.WorkoutPlanExercise
{
    public interface IWorkoutPlanExerciseService:IService<models.WorkoutPlanExercise>
    {
    }
}
