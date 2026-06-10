using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace RepositoryTier.WorkoutPlanExercise.Repositories
{
    public class WorkoutPlanExerciseRepository : Repository<Entities.WorkoutPlanExercise>
        , IWorkoutPlanExerciseRepository
    {
        public WorkoutPlanExerciseRepository(GymManagementDbContext context) : base(context) { }

    }
}
