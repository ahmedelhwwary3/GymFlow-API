using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using models = RepositoryTier.Models;

namespace RepositoryTier.WorkoutPlan.Repositories
{
    public interface IWorkoutPlanRepository : IRepository<models.WorkoutPlan>
    {
    }
}
