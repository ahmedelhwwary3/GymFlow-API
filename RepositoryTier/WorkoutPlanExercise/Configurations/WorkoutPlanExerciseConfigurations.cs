
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using models = RepositoryTier.Models;

namespace RepositoryTier.WorkoutPlanExercise.Configurations
{
    internal class WorkoutPlanExerciseConfigurations : IEntityTypeConfiguration<models.WorkoutPlanExercise>
    {
        public void Configure(EntityTypeBuilder<models.WorkoutPlanExercise> builder)
        {
            builder.HasKey(e => e.Id).HasName("PK__WorkoutP__3214EC073EBFA5A6");

            builder.HasIndex(e => e.WorkoutPlanId, "IX_WorkoutPlanExercises_WorkoutPlanId");

            builder.HasIndex(e => new { e.WorkoutPlanId, e.ExerciseId }, "UQ_WorkoutPlanExercises_WorkoutPlanId_ExerciseId").IsUnique();

            builder.Property(e => e.Notes).HasMaxLength(500);

            builder.HasOne(d => d.Exercise).WithMany(p => p.WorkoutPlanExercises)
                .HasForeignKey(d => d.ExerciseId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_WorkoutPlanExercises_Exercises");

            builder.HasOne(d => d.WorkoutPlan).WithMany(p => p.WorkoutPlanExercises)
                .HasForeignKey(d => d.WorkoutPlanId)
                .OnDelete(DeleteBehavior.NoAction)
                .HasConstraintName("FK_WorkoutPlanExercises_WorkoutPlans");
        }
    }
}
