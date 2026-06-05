using System;
using System.Collections.Generic;

namespace GymManagementAPI.Models;

public partial class Member
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int? CoachId { get; set; }

    public string Address { get; set; } = null!;

    public decimal Height { get; set; }

    public int FitnessGoal { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    public virtual Coach? Coach { get; set; }

    public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();

    public virtual User User { get; set; } = null!;

    public virtual ICollection<WeightRecord> WeightRecords { get; set; } = new List<WeightRecord>();

    public virtual ICollection<WorkoutPlan> WorkoutPlans { get; set; } = new List<WorkoutPlan>();
}
