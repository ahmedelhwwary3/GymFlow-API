
using RepositoryTier.User.Enums;
using System;
using System.Collections.Generic;

namespace RepositoryTier.Entities;

public partial class User
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public enGender Gender { get; set; }

    public DateOnly DateOfBirth { get; set; }

    public string PasswordHash { get; set; } = null!;

    public enUserRole Role { get; set; }

    public string? RefreshTokenHash { get; set; }

    public DateTime? RefreshTokenExpiresAt { get; set; }

    public DateTime? RefreshTokenRevokedAt { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool IsDeleted { get; set; } = false;

    public bool IsActive { get; set; } = true;

    public DateTime? DeletedAt { get; set; }  
}
