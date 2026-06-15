using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.WorkoutPlan.DTOs
{
    public class AddWorkoutPlanExerciseRequest
    {
        [Range(1,int.MaxValue)]
        public int ExerciseId { get; set; }

        [Range(1, int.MaxValue)]
        public int Sets { get; set; }

        [Range(1, int.MaxValue)]
        public int Reps { get; set; }
        public string? Notes { get; set; }
    }
}
