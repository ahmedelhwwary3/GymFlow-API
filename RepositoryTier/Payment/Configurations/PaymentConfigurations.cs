
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq; 
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.Payment.Configurations
{
    internal class PaymentConfigurations : IEntityTypeConfiguration<Entities.Payment>
    {
        public void Configure(EntityTypeBuilder<Entities.Payment> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK__Payments__3214EC07A3F4D955");

            builder.HasIndex(e => e.SubscriptionId, "IX_Payments_SubscriptionId");

            builder.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            builder.Property(e => e.Notes).HasMaxLength(500);

            builder.HasOne(d => d.Subscription).WithMany(p => p.Payments)
                .HasForeignKey(d => d.SubscriptionId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Payments_Subscriptions");
        }
    }
}
