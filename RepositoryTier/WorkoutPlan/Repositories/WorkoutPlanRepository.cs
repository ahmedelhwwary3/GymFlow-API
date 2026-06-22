using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RepositoryTier.API_Configurations;
using RepositoryTier.WorkoutPlan.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace RepositoryTier.WorkoutPlan.Repositories
{
    public class WorkoutPlanRepository : Repository<Entities.WorkoutPlan>, IWorkoutPlanRepository
    {
        protected readonly PaganationOptions PaganationOptions;
        private readonly ILogger<WorkoutPlanRepository> _logger;
        public WorkoutPlanRepository(
            GymManagementDbContext context,
            IOptions<PaganationOptions> paganationOptions,
            ILogger<WorkoutPlanRepository> logger) 
            : base(context) 
        {
            _logger = logger;
            PaganationOptions = paganationOptions.Value;
            if (PaganationOptions == null)
            {
                _logger.LogError("Paganation options is not configured");
                throw new Exception("Paganation options is not configured");
            }
        }

        public async Task<GetWorkoutPlansResponse> 
            GetWorkoutPlansAsync(GetWorkoutPlansRequest request,int? memberId)
        {
            int page = request.Page ?? PaganationOptions.Page;
            int pageSize = request.PageSize ?? PaganationOptions.BigPageSize;
            int count;
            //1.Filter for all Users
            var query = _context.WorkoutPlans
                .IgnoreQueryFilters()
                .AsNoTracking()
                .Where(w =>
                 (
                 string.IsNullOrEmpty(request.Search) ||
                 w.Coach.FullName.Contains(request.Search.Trim()) ||
                 w.Name.Contains(request.Search.Trim())
                 )
                 &&
                 (
                     request.IsActive == null
                     || w.IsActive == request.IsActive)
                 );

            //2.Filter for member only
            if (memberId.HasValue) 
                query = query.Where(w => w.MemberId == memberId);

            //3.Count before paganation
            count = await query.CountAsync();

            //4.Paganation
            var plans = await query
                .Select(w=>new WorkoutPlanResponse()
                {
                    IsActive=w.IsActive,
                    CoachId=w.CoachId,
                    CoachName=w.Coach.FullName,
                    MemberId=w.MemberId,
                    MemberName=w.Member.FullName,
                    PlanName=w.Name
                })
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new GetWorkoutPlansResponse()
            {
                WorkoutPlans= plans,
                Count= count
            };
        }

        public async Task<Entities.WorkoutPlan?> GetLastByMemberIdAsync(int memberId)
        {
            return await _context.WorkoutPlans
               .OrderByDescending(w => w.Id)
               .FirstOrDefaultAsync(w => w.MemberId == memberId);
        }

        public async Task<GetWorkoutPlanByIdResponse?> GetByIdAsync(int Id)
        {
            return await _context.WorkoutPlans
                .IgnoreQueryFilters()
                .AsNoTracking()
                .Where(p => p.Id == Id)
                .Select(p =>
                new GetWorkoutPlanByIdResponse
                {
                    Id = p.Id,
                    IsActive = p.IsActive,
                    CoachId = p.CoachId,
                    CoachName = p.Coach.FullName,
                    MemberId = p.MemberId,
                    MemberName = p.Member.FullName,
                    PlanName = p.Name,

                    Exercises =
                    p.WorkoutPlanExercises
                    .Select(e =>
                    new GetWorkoutPlanExerciseResponse
                    {
                        Id = e.Id,
                        Notes = e.Notes,
                        Reps = e.Reps,
                        Sets = e.Sets
                    }).ToList()
                }).SingleOrDefaultAsync();
        }
    }
}
