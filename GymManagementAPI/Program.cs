
using Microsoft.EntityFrameworkCore;
using RepositoryTier;
using RepositoryTier.Attendance.Repositories;
using RepositoryTier.Coach.Repositories; 
using RepositoryTier.Exercise.Repositories;
using RepositoryTier.Member.Repositories;
using RepositoryTier.Payment.Repositories;
using RepositoryTier.Subscription.Repositories;
using RepositoryTier.User.Repositories;
using RepositoryTier.WeightRecord.Repositories;
using RepositoryTier.WorkoutPlan.Repositories;
using RepositoryTier.WorkoutPlanExercise.Repositories;
using ServiceTier;
using ServiceTier.Attendance;
using ServiceTier.Coach;
using ServiceTier.Configurations;
using ServiceTier.Exercise;
using ServiceTier.Member;
using ServiceTier.Payment;
using ServiceTier.Subscription;
using ServiceTier.User;
using ServiceTier.WeightRecord;
using ServiceTier.WorkoutPlan;
using ServiceTier.WorkoutPlanExercise;
using System.Security.Cryptography;

namespace GymManagement
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();  

            builder.Services.AddDbContext<GymManagementDbContext>(options=>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("connectionString"));
                options.LogTo(Console.WriteLine, LogLevel.Information)
                       .EnableSensitiveDataLogging();
            });

            builder.Services.Configure<JWTOptions>(builder.Configuration.GetSection("JWT"));

            builder.Services.AddScoped<IAttendanceRepository, AttendanceRepository>();
            builder.Services.AddScoped<ICoachRepository, CoachRepository>();
            builder.Services.AddScoped<IExerciseRepository, ExerciseRepository>();
            builder.Services.AddScoped<IMemberRepository, MemberRepository>();
            builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
            builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IWeightRecordRepository, WeightRecordRepository>();
            builder.Services.AddScoped<IWorkoutPlanRepository, WorkoutPlanRepository>();
            builder.Services.AddScoped<IWorkoutPlanExerciseRepository, WorkoutPlanExerciseRepository>();

            builder.Services.AddScoped<IAttendanceService, AttendanceService>();
            builder.Services.AddScoped<ICoachService, CoachService>();
            builder.Services.AddScoped<IExerciseService, ExerciseService>();
            builder.Services.AddScoped<IMemberService, MemberService>();
            builder.Services.AddScoped<IPaymentService, PaymentService>();
            builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IWeightRecordService, WeightRecordService>();
            builder.Services.AddScoped<IWorkoutPlanService, WorkoutPlanService>();
            builder.Services.AddScoped<IWorkoutPlanExerciseService, WorkoutPlanExerciseService>();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();
              
            app.Run(); 
        }
    }
}
