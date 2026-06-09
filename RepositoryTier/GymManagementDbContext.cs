
using Microsoft.EntityFrameworkCore;
using RepositoryTier.Member;
using RepositoryTier.Attendance;
using RepositoryTier.Coach;
using RepositoryTier.Exercise;
using RepositoryTier.Payment;
using RepositoryTier.Subscription;
using RepositoryTier.User;
using RepositoryTier.WeightRecord;
using RepositoryTier.WorkoutPlan;
using RepositoryTier.WorkoutPlanExercise; 
using System;
using System.Collections.Generic;
using models = RepositoryTier.Models;

namespace RepositoryTier;

public partial class GymManagementDbContext : DbContext
{
    public GymManagementDbContext()
    {
    }

    public GymManagementDbContext(DbContextOptions<GymManagementDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<models.Attendance> Attendances { get; set; }

    public virtual DbSet<models.Coach> Coaches { get; set; }

    public virtual DbSet<models.Exercise> Exercises { get; set; }

    public virtual DbSet<models.Member> Members { get; set; }

    public virtual DbSet<models.Payment> Payments { get; set; }

    public virtual DbSet<models.Subscription> Subscriptions { get; set; }

    public virtual DbSet<models.User> Users { get; set; }

    public virtual DbSet<models.WeightRecord> WeightRecords { get; set; }

    public virtual DbSet<models.WorkoutPlan> WorkoutPlans { get; set; }

    public virtual DbSet<models.WorkoutPlanExercise> WorkoutPlanExercises { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Fluent API configurations from separate configuration classes
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GymManagementDbContext).Assembly);
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
