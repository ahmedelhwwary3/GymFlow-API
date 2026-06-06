 
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RepositoryTier.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.Data.Configurations
{
    internal class SubscriptionConfigurations : IEntityTypeConfiguration<Subscription>
    {
        public void Configure(EntityTypeBuilder<Subscription> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK__Subscrip__3214EC07EB458BD3");

            builder.HasIndex(e => e.CreatedByUserId, "IX_Subscriptions_CreatedByUserId");

            builder.HasIndex(e => e.MemberId, "IX_Subscriptions_MemberId");

            builder.Property(e => e.Price).HasColumnType("decimal(10, 2)");

            builder.HasOne(d => d.CreatedByUser).WithMany(p => p.Subscriptions)
                .HasForeignKey(d => d.CreatedByUserId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Subscriptions_Users");

            builder.HasOne(d => d.Member).WithMany(p => p.Subscriptions)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Subscriptions_Members");
        }
    }
}
