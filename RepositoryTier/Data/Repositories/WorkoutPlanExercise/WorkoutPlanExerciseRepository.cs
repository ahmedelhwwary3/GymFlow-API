using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.Data.Repositories.WorkoutPlanExercise
{
    internal class WorkoutPlanExerciseRepository : Repository<GymManagementAPI.Models.WorkoutPlanExercise>
        , IWorkoutPlanExerciseRepository
    {
        public WorkoutPlanExerciseRepository(GymManagementDbContext context) : base(context) { }

    }
}
