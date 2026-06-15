using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.WorkoutPlan.DTOs
{
    public class GetWorkoutPlanByIdResponse
    {
        public int Id { get; set; }
        public string PlanName { get; set; }
        public bool IsActive { get; set; }
        public int MemberId { get; set; }
        public string MemberName { get; set; } 
        public int CoachId { get; set; }
        public string CoachName { get; set; }

        public List<GetWorkoutPlanExerciseResponse> Exercises { get; set; }

    }
}
