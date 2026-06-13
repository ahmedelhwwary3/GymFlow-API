

using RepositoryTier.Subscription.Enums;

namespace RepositoryTier.Entities;

public partial class Subscription
{
    public int Id { get; set; }

    public int MemberId { get; set; }

    public int CoachId { get; set; }

    public enSubscriptionPlan SubscriptionPlan { get; set; }

    public decimal Price { get; set; }

    public DateOnly StartDate { get; set; } 

    public DateOnly EndDate { get; set; }

    public DateOnly? FreezeStartDate { get; set; } = null;

    public DateOnly? FreezeEndDate { get; set; } = null;

    public DateTime CreatedAt { get; set; } 

    public int CreatedByUserId { get; set; }

    public bool IsDeleted { get; set; } = false;

    public DateTime? DeletedAt { get; set; } = null;

    public virtual User CreatedByUser { get; set; } = null!;

    public virtual Member Member { get; set; } = null!;

    public virtual Coach Coach { get; set; } = null!;

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public Subscription()
    {
        StartDate = DateOnly.FromDateTime(DateTime.UtcNow);
        CreatedAt = DateTime.UtcNow;
    }
}
