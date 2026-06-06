using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.Data.Repositories.WorkoutPlanExercise
{
    public class WorkoutPlanExerciseRepository : Repository<RepositoryTier.Models.WorkoutPlanExercise>
        , IWorkoutPlanExerciseRepository
    {
        public WorkoutPlanExerciseRepository(GymManagementDbContext context) : base(context) { }

    }
}
