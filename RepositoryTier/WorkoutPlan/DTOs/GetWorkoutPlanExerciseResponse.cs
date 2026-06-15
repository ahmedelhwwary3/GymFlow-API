using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.WorkoutPlan.DTOs
{
    public class GetWorkoutPlanExerciseResponse
    { 
        public int Id { get; set; } 
        public int Sets { get; set; } 
        public int Reps { get; set; }
        public string? Notes { get; set; }
    }
}
