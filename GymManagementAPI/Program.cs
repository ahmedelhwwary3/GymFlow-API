 
using GymManagementAPI.Authorization.OwnerOrAdmin;
using GymManagementAPI.Authorization.SubscriptionOwnerOrAdmin;
using GymManagementAPI.Authorization.WorkoutPlanOwnerOrAdmin;
using GymManagementAPI.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore; 
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RepositoryTier;
using RepositoryTier.API_Configurations;
using RepositoryTier.Attendance.Repositories;
using RepositoryTier.Coach.Repositories;
using RepositoryTier.Dashboard.Repositories;
using RepositoryTier.Exercise.Repositories;
using RepositoryTier.Member.Repositories;
using RepositoryTier.Payment.Repositories;
using RepositoryTier.Subscription.Repositories;
using RepositoryTier.User.Repositories;
using RepositoryTier.WeightRecord.Repositories;
using RepositoryTier.WorkoutPlan.Repositories;
using RepositoryTier.WorkoutPlanExercise.Repositories; 
using ServiceTier.Attendance;
using ServiceTier.Coach;
using ServiceTier.Dashboard;
using ServiceTier.Exercise;
using ServiceTier.Member;
using ServiceTier.Payment;
using ServiceTier.Subscription;
using ServiceTier.User;
using ServiceTier.WeightRecord;
using ServiceTier.WorkoutPlan;
using ServiceTier.WorkoutPlanExercise;
using System.Security.Claims;
using System.Text;
using System.Threading.RateLimiting;

namespace GymManagement
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            var jwt = builder.Configuration.GetSection("JWT");

            builder.Services.AddRateLimiter(options =>
            {
                options.AddPolicy(Policies.TokenBucketAuthLimiter, httpContext =>
                {
                    var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

                    return RateLimitPartition.GetTokenBucketLimiter(
                        partitionKey: ip,
                        factory: _ => new TokenBucketRateLimiterOptions
                        {
                            TokenLimit = 10, // trials
                            TokensPerPeriod = 1, // Per 1 min
                            ReplenishmentPeriod = TimeSpan.FromSeconds(6), // new trial every 1/5 min
                            AutoReplenishment = true,
                            QueueLimit = 0
                        });
                });

                options.AddPolicy(Policies.SlidingWindowAuthLimiter, httpContext =>
                {
                    var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

                    return RateLimitPartition.GetSlidingWindowLimiter(
                        partitionKey: ip,
                        factory: _ => new SlidingWindowRateLimiterOptions
                        {
                            PermitLimit = 5,                     // أقصى عدد طلبات
                            Window = TimeSpan.FromMinutes(1),   // آخر دقيقة
                            SegmentsPerWindow = 6,              // تقسيم الدقيقة إلى 6 أجزاء (10 ثوانٍ لكل جزء)
                            QueueLimit = 0
                        });
                });

                options.AddPolicy(Policies.FixedWindowAuthLimiter, httpContext =>
                {
                    var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

                    return RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: ip,
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 5,                    // 5 requests
                            Window = TimeSpan.FromMinutes(1),  // per minute
                            QueueLimit = 0
                        });
                });

                options.OnRejected = async (context, cancellationToken) =>
                {
                    context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;

                    await context.HttpContext.Response.WriteAsync(
                        "Too many login attempts. Please try again later.",
                        cancellationToken);
                };
            });
             
            builder.Services.Configure<JWTOptions>(jwt);
            builder.Services.Configure<PaganationOptions>(builder.Configuration.GetSection("Paganation"));

            builder.Services.AddDbContext<GymManagementDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("connectionString"));
                options.LogTo(Console.WriteLine, LogLevel.Information)
                       .EnableSensitiveDataLogging();
            });
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
            builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();

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
            builder.Services.AddScoped<IDashboardService, DashboardService>();

            builder.Services.AddScoped<IAuthorizationHandler, SubscriptionOwnerOrAdminHandler>(); // Because it uses context object in ctor (Scoped)
            builder.Services.AddScoped<IAuthorizationHandler, WorkoutPlanOwnerOrAdminHandler>();// Because it uses context object in ctor (Scoped)
            builder.Services.AddSingleton<IAuthorizationHandler, OwnerOrAdminHandler>();

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy(Policies.Cors, policy =>
                {
                    policy.WithOrigins(
                    "http://localhost:5059",
                    "https://localhost:7041")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                }
                );
            });

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                string? secretKey = builder.Configuration["GYM_SECRET_KEY"];
                if (string.IsNullOrEmpty(secretKey))
                {
                    throw new Exception("JWT secret key is not configured.");
                }
                // TokenValidationParameters define how incoming JWTs will be validated.
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    // Ensures the token was issued by a trusted issuer.
                    ValidateIssuer = true,
                    // Ensures the token is intended for this API (audience check).
                    ValidateAudience = true,
                    // Ensures the token has not expired.
                    ValidateLifetime = true,
                    // Ensures the token signature is valid and was signed by the API.
                    ValidateIssuerSigningKey = true,
                    // The expected issuer value (must match the issuer used when creating the JWT).
                    ValidIssuer = jwt["Issuer"],
                    // The expected audience value (must match the audience used when creating the JWT).
                    ValidAudience = jwt["Audience"],
                    // The secret key used to validate the JWT signature.
                    // This must be the same key used when generating the token.
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(secretKey))
                };
            });

            builder.Services.AddAuthorization(options =>
            { 

                options.AddPolicy(Policies.SubscriptionOwnerOrAdmin, policy =>
                {
                    policy.AddRequirements(new SubscriptionOwnerOrAdminRequirement());
                });

                options.AddPolicy(Policies.WorkoutPlanOwnerOrAdmin, policy =>
                {
                    policy.AddRequirements(new WorkoutPlanOwnerOrAdminRequirement());
                });

                options.AddPolicy(Policies.OwnerOrAdmin, policy =>
                {
                    policy.AddRequirements(new OwnerOrAdminRequirement());
                });
            });

            builder.Services.AddSwaggerGen();
            // Register Swagger generator and customize its behavior.
            builder.Services.AddSwaggerGen(options =>
            {
                // 1.create the Button in Swagger UI to input the JWT token for authenticated requests.
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    // The name of the HTTP header where the token will be sent.
                    Name = "Authorization",

                    // Indicates this is an HTTP authentication scheme.
                    Type = SecuritySchemeType.Http,

                    // Specifies the authentication scheme name.
                    // Must be exactly "Bearer" for JWT Bearer tokens.
                    Scheme = "Bearer",

                    // Optional metadata to describe the token format.
                    BearerFormat = "JWT",

                    // Specifies that the token is sent in the request header.
                    In = ParameterLocation.Header,

                    // Text shown in Swagger UI to guide the user.
                    Description = "Enter: Bearer {your JWT token}"
                });

                // 2.Apply [authorize] to endpoints with token sent to Button in Swagger UI.
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            // Reference the previously defined "Bearer" security scheme.
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },


                        // No scopes are required for JWT Bearer authentication.
                        // This array is empty because JWT does not use OAuth scopes here.
                        new string[] {}
                    }
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler(errorApp =>
                {
                    errorApp.Run(async context =>
                    {
                        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        context.Response.ContentType = "application/json";
                        var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();
                        var exception = exceptionFeature?.Error;
                        var IPAddress = context.Connection.RemoteIpAddress?.ToString()??"Anonymous";

                        app.Logger.LogError(exception,
                            "Unhandled exception occurred. Path={Path}, IP={ip}",
                            context.Request.Path, IPAddress);

                        await context.Response.WriteAsJsonAsync(new
                    {
                        Message = "An unexpected error occurred."
                    });
                    });
                }); ;
            }
             
            app.UseHttpsRedirection();

            app.UseCors(Policies.Cors);

            app.UseHttpsRedirection();

            app.UseRateLimiter();

            app.UseAuthentication();

            app.UseAuthorization();

            // Global 403 logging middleware (place it HERE)
            app.Use(async (context, next) =>
            {
                await next();


                if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
                {
                    // 1- logged user (OR) 2- public api
                    var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "anonymous";
                    var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
                    var path = context.Request.Path.ToString();// because we don't know action name (global)


                    // ✅ Centralized security log for authorization abuse
                    app.Logger.LogWarning(
                        "Forbidden access. UserId={UserId}, Path={Path}, IP={IP} ",
                        userId,
                        path,
                        ip
                    );
                }
            });

            app.MapControllers();
              
            app.Run(); 
        }
    }
}
