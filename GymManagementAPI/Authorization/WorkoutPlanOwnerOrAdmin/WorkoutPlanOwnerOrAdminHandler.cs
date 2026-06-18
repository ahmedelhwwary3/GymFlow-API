using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using RepositoryTier;
using RepositoryTier.User.Enums;
using System.Security.Claims;

namespace GymManagementAPI.Authorization.WorkoutPlanOwnerOrAdmin
{
    public class WorkoutPlanOwnerOrAdminHandler : AuthorizationHandler<WorkoutPlanOwnerOrAdminRequirement, int>
    {
        private readonly GymManagementDbContext _context;
        public WorkoutPlanOwnerOrAdminHandler(GymManagementDbContext context)
        {
            _context = context;
        }
        protected override async Task HandleRequirementAsync
            (AuthorizationHandlerContext context, WorkoutPlanOwnerOrAdminRequirement requirement, int requestWorkoutPlanId)
        {
            string? tokenUserId = context.User.FindFirstValue(ClaimTypes.NameIdentifier); 
            if (string.IsNullOrEmpty(tokenUserId))
            {
                context.Fail();
                return;
            }

            bool isAdmin = context.User.IsInRole($"{(int)enUserRole.Admin}");
            int userId = Convert.ToInt32(tokenUserId);
            bool isMemberWorkoutPlan = await _context.WorkoutPlans
                .AnyAsync(s => 
                s.Id == requestWorkoutPlanId && s.MemberId == userId);

            if (isMemberWorkoutPlan || isAdmin)
            {
                context.Succeed(requirement);
            }
             
            return;
        }
    }
}
