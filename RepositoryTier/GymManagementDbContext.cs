
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

    public virtual DbSet<Entities.Attendance> Attendances { get; set; }

    public virtual DbSet<Entities.Coach> Coaches { get; set; }

    public virtual DbSet<Entities.Exercise> Exercises { get; set; }

    public virtual DbSet<Entities.Member> Members { get; set; }

    public virtual DbSet<Entities.Payment> Payments { get; set; }

    public virtual DbSet<Entities.Subscription> Subscriptions { get; set; }

    public virtual DbSet<Entities.User> Users { get; set; }

    public virtual DbSet<Entities.WeightRecord> WeightRecords { get; set; }

    public virtual DbSet<Entities.WorkoutPlan> WorkoutPlans { get; set; }

    public virtual DbSet<Entities.WorkoutPlanExercise> WorkoutPlanExercises { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Fluent API configurations from separate configuration classes
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GymManagementDbContext).Assembly);
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
