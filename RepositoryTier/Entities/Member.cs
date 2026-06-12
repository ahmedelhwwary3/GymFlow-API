using System;
using System.Collections.Generic;
using RepositoryTier.User.Enums;
using RepositoryTier.Member.Enums;

namespace RepositoryTier.Entities;

public partial class Member:User
{   

    public int? CoachId { get; set; }

    public string Address { get; set; } = null!;

    public decimal Height { get; set; }

    public enMemberFitnessGoal FitnessGoal { get; set; } 


    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    public virtual Coach? Coach { get; set; }

    public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>(); 

    public virtual ICollection<WeightRecord> WeightRecords { get; set; } = new List<WeightRecord>();

    public virtual ICollection<WorkoutPlan> WorkoutPlans { get; set; } = new List<WorkoutPlan>();

    public Member()
    {
        base.Role = enUserRole.Member;
    }
}
