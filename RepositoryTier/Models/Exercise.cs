using System;
using System.Collections.Generic;

namespace RepositoryTier.Models;

public partial class Exercise
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int TargetMuscleGroup { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual ICollection<WorkoutPlanExercise> WorkoutPlanExercises { get; set; } = new List<WorkoutPlanExercise>();
}
