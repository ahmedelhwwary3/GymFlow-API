
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using models = RepositoryTier.Models;
using System.Threading.Tasks;

namespace RepositoryTier.WeightRecord.Configurations
{
    internal class WeightRecordConfigurations : IEntityTypeConfiguration<models.WeightRecord>
    {
        public void Configure(EntityTypeBuilder<models.WeightRecord> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK__WeightRe__3214EC07581F8344");

            builder.HasIndex(e => e.MemberId, "IX_WeightRecords_MemberId");

            builder.HasIndex(e => new { e.MemberId, e.RecordedDate }, "UQ_WeightRecords_MemberId_RecordedDate").IsUnique();

            builder.Property(e => e.Notes).HasMaxLength(500);
            builder.Property(e => e.Weight).HasColumnType("decimal(5, 2)");

            builder.HasOne(d => d.Member).WithMany(p => p.WeightRecords)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_WeightRecords_Members");
        }
    }
}
