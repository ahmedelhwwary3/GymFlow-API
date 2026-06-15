using RepositoryTier.Coach.DTOs;
using RepositoryTier.WorkoutPlan.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.WorkoutPlan.Results
{
    public class AddWorkoutPlanResult
    {
        public int? Id { get; set; }
        public enAddWorkoutPlanStatus Status { get; set; }
        public AddWorkoutPlanResult(enAddWorkoutPlanStatus status,int? Id=null)
        {
            this.Id = Id;
            this.Status= status;
        }
    }
}
