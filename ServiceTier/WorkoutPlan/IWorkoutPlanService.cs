using RepositoryTier.WorkoutPlan.DTOs;
using RepositoryTier.WorkoutPlan.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
namespace ServiceTier.WorkoutPlan
{
    public interface IWorkoutPlanService: IService<RepositoryTier.Entities.WorkoutPlan>
    {
        Task<GetWorkoutPlansResponse> GetWorkoutPlansAsync(GetWorkoutPlansRequest request,int? memberId);
        Task<AddWorkoutPlanResult> AddWithExercisesAsync(AddWorkoutPlanRequest request);
    }
}
