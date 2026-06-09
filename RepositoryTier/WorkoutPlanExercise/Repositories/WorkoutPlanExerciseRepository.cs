using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using models = RepositoryTier.Models;

namespace RepositoryTier.WorkoutPlanExercise.Repositories
{
    public class WorkoutPlanExerciseRepository : Repository<models.WorkoutPlanExercise>
        , IWorkoutPlanExerciseRepository
    {
        public WorkoutPlanExerciseRepository(GymManagementDbContext context) : base(context) { }

    }
}
