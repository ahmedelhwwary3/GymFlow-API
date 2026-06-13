
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace RepositoryTier.Subscription.Configurations
{
    internal class SubscriptionConfigurations : IEntityTypeConfiguration<Entities.Subscription>
    {
        public void Configure(EntityTypeBuilder<Entities.Subscription> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK__Subscrip__3214EC07EB458BD3"); 

            builder.HasIndex(e => e.MemberId, "IX_Subscriptions_MemberId");

            builder.Property(e => e.Price).HasColumnType("decimal(10, 2)");

            builder.HasQueryFilter(e => !e.IsDeleted); 

            builder.HasOne(d => d.Member).WithMany(p => p.Subscriptions)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Subscriptions_Members");

            builder.HasOne(d => d.Coach).WithMany(p => p.Subscriptions)
                .HasForeignKey(d => d.CoachId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Subscriptions_Coaches");
        }
    }
}
