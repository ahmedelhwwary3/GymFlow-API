
using RepositoryTier.CustomAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.WorkoutPlan.DTOs
{
    public class AddWorkoutPlanRequest
    {
        [Required]
        public string MemberIdentifier { get; set; }
        public int? CoachId { get; set; }
        public string? Notes { get; set; }

        [Required]
        public string PlanName { get; set; }

        [NotEmptyList]
        public List<AddWorkoutPlanExerciseRequest> Exercises {  get; set; }


    }
}
