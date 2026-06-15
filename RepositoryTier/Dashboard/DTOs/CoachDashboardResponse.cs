using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.Dashboard.DTOs
{
    public class CoachDashboardResponse
    {
        public int AssignedMembers { get; set; }
        public int ActiveMembers { get; set; }
        public int InactiveMembers { get => AssignedMembers - ActiveMembers; }

        public int WorkoutPlansCreated { get; set; }
        public int ActiveWorkoutPlans { get; set; }
        public int InctiveWorkoutPlans { get => WorkoutPlansCreated - ActiveWorkoutPlans; }
    }
}
