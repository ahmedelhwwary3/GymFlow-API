using System;
using System.Collections.Generic;

namespace GymManagementAPI.Models;

public partial class Payment
{
    public int Id { get; set; }

    public int SubscriptionId { get; set; }

    public decimal Amount { get; set; }

    public DateOnly PaymentDate { get; set; }

    public string? Notes { get; set; }

    public virtual Subscription Subscription { get; set; } = null!;
}
