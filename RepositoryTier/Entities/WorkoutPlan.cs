
using RepositoryTier.WorkoutPlan.Enums;
namespace RepositoryTier.Entities;

public partial class WorkoutPlan
{
    public int Id { get; set; }

    public int MemberId { get; set; }

    public int CoachId { get; set; }

    public string Name { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Coach Coach { get; set; } = null!;

    public virtual Member Member { get; set; } = null!;

    public virtual ICollection<WorkoutPlanExercise> WorkoutPlanExercises { get; set; } = new List<WorkoutPlanExercise>();
}
