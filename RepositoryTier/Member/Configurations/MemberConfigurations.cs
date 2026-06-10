
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using models = RepositoryTier.Models;

namespace RepositoryTier.Member.Configurations
{
    internal class MemberConfigurations : IEntityTypeConfiguration<models.Member>
    {
        public void Configure(EntityTypeBuilder<models.Member> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK__Members__3214EC07ADBAE99E");

            builder.HasIndex(e => e.CoachId, "IX_Members_CoachId"); 

            builder.Property(e => e.Address).HasMaxLength(300);
            builder.Property(e => e.Height).HasColumnType("decimal(5, 2)");

            builder.HasOne(d => d.Coach).WithMany(p => p.Members)
                .HasForeignKey(d => d.CoachId)
                .HasConstraintName("FK_Members_Coaches"); 
        }
    }
}
