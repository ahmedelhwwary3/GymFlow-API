using System;
using System.Collections.Generic;

namespace GymManagementAPI.Models;

public partial class Coach
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int Specialization { get; set; }

    public DateOnly HireDate { get; set; }

    public decimal Salary { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<Member> Members { get; set; } = new List<Member>();

    public virtual User User { get; set; } = null!;

    public virtual ICollection<WorkoutPlan> WorkoutPlans { get; set; } = new List<WorkoutPlan>();
}
