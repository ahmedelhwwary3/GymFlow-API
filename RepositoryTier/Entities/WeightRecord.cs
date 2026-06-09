 

namespace RepositoryTier.Models;

public partial class WeightRecord
{
    public int Id { get; set; }

    public int MemberId { get; set; }

    public decimal Weight { get; set; }

    public DateOnly RecordedDate { get; set; }

    public string? Notes { get; set; }

    public virtual Member Member { get; set; } = null!;
}
