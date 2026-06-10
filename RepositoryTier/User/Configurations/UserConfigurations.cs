
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace RepositoryTier.User.Configurations
{
    internal class UserConfigurations : IEntityTypeConfiguration<Entities.User>
    {
        public void Configure(EntityTypeBuilder<Entities.User> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK__Users__3214EC0731A6BD8E");

            builder.HasIndex(e => e.Email, "IX_Users_Email").IsUnique();

            builder.HasIndex(e => e.Phone, "IX_Users_Phone").IsUnique();

            builder.HasIndex(e => e.Email, "UQ_Users_Email").IsUnique();

            builder.HasIndex(e => e.Phone, "UQ_Users_Phone").IsUnique();

            builder.HasQueryFilter(e=>!e.IsDeleted && e.DeletedAt != null);

            builder.UseTptMappingStrategy();

            builder.Property(e => e.Email).HasMaxLength(255);
            builder.Property(e => e.IsActive).HasDefaultValue(true);
            builder.Property(e => e.IsDeleted).HasDefaultValue(false);
            builder.Property(e => e.FullName).HasMaxLength(150);
            builder.Property(e => e.PasswordHash).HasMaxLength(500);
            builder.Property(e => e.Phone).HasMaxLength(20);
            builder.Property(e => e.RefreshTokenHash).HasMaxLength(500);
        }
    }
}
