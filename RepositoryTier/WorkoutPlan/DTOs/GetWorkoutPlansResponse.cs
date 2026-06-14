using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.WorkoutPlan.DTOs
{
    public class GetWorkoutPlansResponse
    {
        public List<WorkoutPlanResponse> WorkoutPlans { get; set; }
        public int Count { get; set; }
    }
}
