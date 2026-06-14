using RepositoryTier.WorkoutPlan.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace RepositoryTier.WorkoutPlan.Repositories
{
    public interface IWorkoutPlanRepository : IRepository<Entities.WorkoutPlan>
    {
        Task<GetWorkoutPlansResponse> GetWorkoutPlansAsync(GetWorkoutPlansRequest request,int? memberId);
    }
}
