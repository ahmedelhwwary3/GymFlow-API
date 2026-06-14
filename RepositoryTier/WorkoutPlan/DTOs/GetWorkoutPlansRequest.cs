using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.WorkoutPlan.DTOs
{
    public class GetWorkoutPlansRequest
    { 
        public string? Search { get; set; }
        public bool? IsActive { get; set; }

        public int? Page {  get; set; }
        public int? PageSize { get; set; }
    }
}
