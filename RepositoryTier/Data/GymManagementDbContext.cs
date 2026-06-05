using System;
using System.Collections.Generic;
using GymManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace RepositoryTier.Data;

public partial class GymManagementDbContext : DbContext
{
    public GymManagementDbContext()
    {
    }

    public GymManagementDbContext(DbContextOptions<GymManagementDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Attendance> Attendances { get; set; }

    public virtual DbSet<Coach> Coaches { get; set; }

    public virtual DbSet<Exercise> Exercises { get; set; }

    public virtual DbSet<Member> Members { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Subscription> Subscriptions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<WeightRecord> WeightRecords { get; set; }

    public virtual DbSet<WorkoutPlan> WorkoutPlans { get; set; }

    public virtual DbSet<WorkoutPlanExercise> WorkoutPlanExercises { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Attendance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Attendan__3214EC0794DDB513");

            entity.HasIndex(e => e.MemberId, "IX_Attendances_MemberId");

            entity.HasIndex(e => new { e.MemberId, e.AttendanceDate }, "UQ_Attendances_MemberId_AttendanceDate").IsUnique();

            entity.Property(e => e.Notes).HasMaxLength(500);

            entity.HasOne(d => d.Member).WithMany(p => p.Attendances)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Attendances_Members");
        });

        modelBuilder.Entity<Coach>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Coaches__3214EC07E0D177E5");

            entity.HasIndex(e => e.UserId, "UQ_Coaches_UserId").IsUnique();

            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Salary).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.User).WithOne(p => p.Coach)
                .HasForeignKey<Coach>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Coaches_Users");
        });

        modelBuilder.Entity<Exercise>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Exercise__3214EC07938161E8");

            entity.HasIndex(e => e.Name, "UQ_Exercises_Name").IsUnique();

            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.Name).HasMaxLength(150);
        });

        modelBuilder.Entity<Member>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Members__3214EC07ADBAE99E");

            entity.HasIndex(e => e.CoachId, "IX_Members_CoachId");

            entity.HasIndex(e => e.UserId, "UQ_Members_UserId").IsUnique();

            entity.Property(e => e.Address).HasMaxLength(300);
            entity.Property(e => e.Height).HasColumnType("decimal(5, 2)");

            entity.HasOne(d => d.Coach).WithMany(p => p.Members)
                .HasForeignKey(d => d.CoachId)
                .HasConstraintName("FK_Members_Coaches");

            entity.HasOne(d => d.User).WithOne(p => p.Member)
                .HasForeignKey<Member>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Members_Users");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Payments__3214EC07A3F4D955");

            entity.HasIndex(e => e.SubscriptionId, "IX_Payments_SubscriptionId");

            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Notes).HasMaxLength(500);

            entity.HasOne(d => d.Subscription).WithMany(p => p.Payments)
                .HasForeignKey(d => d.SubscriptionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Payments_Subscriptions");
        });

        modelBuilder.Entity<Subscription>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Subscrip__3214EC07EB458BD3");

            entity.HasIndex(e => e.CreatedByUserId, "IX_Subscriptions_CreatedByUserId");

            entity.HasIndex(e => e.MemberId, "IX_Subscriptions_MemberId");

            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.CreatedByUser).WithMany(p => p.Subscriptions)
                .HasForeignKey(d => d.CreatedByUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Subscriptions_Users");

            entity.HasOne(d => d.Member).WithMany(p => p.Subscriptions)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Subscriptions_Members");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC0731A6BD8E");

            entity.HasIndex(e => e.Email, "IX_Users_Email").IsUnique();

            entity.HasIndex(e => e.Phone, "IX_Users_Phone").IsUnique();

            entity.HasIndex(e => e.Email, "UQ_Users_Email").IsUnique();

            entity.HasIndex(e => e.Phone, "UQ_Users_Phone").IsUnique();

            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FullName).HasMaxLength(150);
            entity.Property(e => e.PasswordHash).HasMaxLength(500);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.RefreshTokenHash).HasMaxLength(500);
        });

        modelBuilder.Entity<WeightRecord>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__WeightRe__3214EC07581F8344");

            entity.HasIndex(e => e.MemberId, "IX_WeightRecords_MemberId");

            entity.HasIndex(e => new { e.MemberId, e.RecordedDate }, "UQ_WeightRecords_MemberId_RecordedDate").IsUnique();

            entity.Property(e => e.Notes).HasMaxLength(500);
            entity.Property(e => e.Weight).HasColumnType("decimal(5, 2)");

            entity.HasOne(d => d.Member).WithMany(p => p.WeightRecords)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WeightRecords_Members");
        });

        modelBuilder.Entity<WorkoutPlan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__WorkoutP__3214EC07BB00F0C2");

            entity.HasIndex(e => e.CoachId, "IX_WorkoutPlans_CoachId");

            entity.HasIndex(e => e.MemberId, "IX_WorkoutPlans_MemberId");

            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(150);

            entity.HasOne(d => d.Coach).WithMany(p => p.WorkoutPlans)
                .HasForeignKey(d => d.CoachId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WorkoutPlans_Coaches");

            entity.HasOne(d => d.Member).WithMany(p => p.WorkoutPlans)
                .HasForeignKey(d => d.MemberId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WorkoutPlans_Members");
        });

        modelBuilder.Entity<WorkoutPlanExercise>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__WorkoutP__3214EC073EBFA5A6");

            entity.HasIndex(e => e.WorkoutPlanId, "IX_WorkoutPlanExercises_WorkoutPlanId");

            entity.HasIndex(e => new { e.WorkoutPlanId, e.ExerciseId }, "UQ_WorkoutPlanExercises_WorkoutPlanId_ExerciseId").IsUnique();

            entity.Property(e => e.Notes).HasMaxLength(500);

            entity.HasOne(d => d.Exercise).WithMany(p => p.WorkoutPlanExercises)
                .HasForeignKey(d => d.ExerciseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WorkoutPlanExercises_Exercises");

            entity.HasOne(d => d.WorkoutPlan).WithMany(p => p.WorkoutPlanExercises)
                .HasForeignKey(d => d.WorkoutPlanId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_WorkoutPlanExercises_WorkoutPlans");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
