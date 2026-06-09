
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using models = RepositoryTier.Models;

namespace RepositoryTier.WorkoutPlan.Configurations
{
    internal class WorkoutPlanConfigurations : IEntityTypeConfiguration<models.WorkoutPlan>
    {
        public void Configure(EntityTypeBuilder<models.WorkoutPlan> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK__WorkoutP__3214EC07BB00F0C2");

            builder.HasIndex(e => e.CoachId, "IX_WorkoutPlans_CoachId");

            builder.HasIndex(e => e.MemberId, "IX_WorkoutPlans_MemberId");

            builder.Property(e => e.IsActive).HasDefaultValue(true);
            builder.Property(e => e.Name).HasMaxLength(150);

            builder.HasOne(d => d.Coach).WithMany(p => p.WorkoutPlans)
                .HasForeignKey(d => d.CoachId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_WorkoutPlans_Coaches");

            builder.HasOne(d => d.Member).WithMany(p => p.WorkoutPlans)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_WorkoutPlans_Members");
        }
    }
}
