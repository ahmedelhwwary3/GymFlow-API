using RepositoryTier.Subscription.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.Dashboard.DTOs
{
    public class MemberDashboardResponse
    {
        public int? CoachId { get; set; } 
        public string? CoachName { get; set; }

        public enSubscriptionPlan? SubscriptionPlan { get; set; }
        public enSubscriptonStatus? SubscriptonStatus { get; set; }
        public DateOnly? SubscriptionEndDate { get; set; } 
        public int? RemainingDays { get; set;  }
         
        public int? CurrentWorkoutPlanId { get; set; } 
        public string? CurrentWorkoutPlanName { get; set; }

        public int? ExercisesCount { get; set; }

        public int? AttendanceThisMonth { get; set; } 
        public DateOnly? LastAttendanceDate { get; set; }

       
    }
}
