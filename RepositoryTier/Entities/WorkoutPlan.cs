
 
namespace RepositoryTier.Entities;

public partial class WorkoutPlan
{
    public int Id { get; set; }

    public int MemberId { get; set; }

    public int CoachId { get; set; }

    public string Name { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; }=DateTime.UtcNow;

    public virtual Coach Coach { get; set; } = null!;

    public virtual Member Member { get; set; } = null!;

    public virtual ICollection<WorkoutPlanExercise> WorkoutPlanExercises { get; set; } = new List<WorkoutPlanExercise>();

    public WorkoutPlan()
    {
        CreatedAt = DateTime.UtcNow;
        IsActive = true;
    }
}
