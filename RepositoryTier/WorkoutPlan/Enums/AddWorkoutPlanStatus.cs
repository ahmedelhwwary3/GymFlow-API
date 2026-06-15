using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.WorkoutPlan.Enums
{
    public enum enAddWorkoutPlanStatus
    {
        CoachNotFound=1,
        ExerciseRepeated=2,
        ExerciseNotFound=3,
        MemberNotFound=4,
        Succeeded=5,
        CoachInactive=6
    }
}
