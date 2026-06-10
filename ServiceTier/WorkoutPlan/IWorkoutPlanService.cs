using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
namespace ServiceTier.WorkoutPlan
{
    public interface IWorkoutPlanService: IService<RepositoryTier.Entities.WorkoutPlan>
    {
    }
}
