using Microsoft.EntityFrameworkCore;
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
        public WorkoutPlanRepository(
            GymManagementDbContext context,
            IOptions<PaganationOptions> paganationOptions) 
            : base(context) 
        {
            PaganationOptions = paganationOptions.Value;
            if (PaganationOptions == null)
                throw new Exception("Paganation options is not configured");
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
               .OrderBy(w => w.Id)
               .LastAsync(w => w.MemberId == memberId);
        }
    }
}
