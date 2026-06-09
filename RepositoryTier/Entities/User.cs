
using System;
using System.Collections.Generic;

namespace RepositoryTier.Models;

public partial class User
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public int Gender { get; set; }

    public DateOnly DateOfBirth { get; set; }

    public string PasswordHash { get; set; } = null!;

    public int Role { get; set; }

    public string? RefreshTokenHash { get; set; }

    public DateTime? RefreshTokenExpiresAt { get; set; }

    public DateTime? RefreshTokenRevokedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; }

    public bool IsActive { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual Coach? Coach { get; set; }

    public virtual Member? Member { get; set; }

    public virtual ICollection<Subscription> Subscriptions { get; set; } = new List<Subscription>();
}
