 

namespace RepositoryTier.Entities;

public partial class Subscription
{
    public int Id { get; set; }

    public int MemberId { get; set; }

    public int SubscriptionPlan { get; set; }

    public decimal Price { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public DateOnly? FreezeStartDate { get; set; }

    public DateOnly? FreezeEndDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public int CreatedByUserId { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual User CreatedByUser { get; set; } = null!;

    public virtual Member Member { get; set; } = null!;

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
