 
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
    internal class MemberConfigurations : IEntityTypeConfiguration<Member>
    {
        public void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK__Members__3214EC07ADBAE99E");

            builder.HasIndex(e => e.CoachId, "IX_Members_CoachId");

            builder.HasIndex(e => e.UserId, "UQ_Members_UserId").IsUnique();

            builder.Property(e => e.Address).HasMaxLength(300);
            builder.Property(e => e.Height).HasColumnType("decimal(5, 2)");

            builder.HasOne(d => d.Coach).WithMany(p => p.Members)
                .HasForeignKey(d => d.CoachId)
                .HasConstraintName("FK_Members_Coaches");

            builder.HasOne(d => d.User).WithOne(p => p.Member)
                .HasForeignKey<Member>(d => d.UserId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Members_Users");
        }
    }
}
