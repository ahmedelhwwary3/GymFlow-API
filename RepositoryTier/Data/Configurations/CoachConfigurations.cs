using GymManagementAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.Data.Configurations
{
    internal class CoachConfigurations : IEntityTypeConfiguration<Coach>
    {
        public void Configure(EntityTypeBuilder<Coach> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK__Coaches__3214EC07E0D177E5");

            builder.HasIndex(e => e.UserId, "UQ_Coaches_UserId").IsUnique();

            builder.Property(e => e.IsActive).HasDefaultValue(true);
            builder.Property(e => e.Salary).HasColumnType("decimal(18, 2)");

            builder.HasOne(d => d.User).WithOne(p => p.Coach)
                .HasForeignKey<Coach>(d => d.UserId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_Coaches_Users");
        }
    }
}
