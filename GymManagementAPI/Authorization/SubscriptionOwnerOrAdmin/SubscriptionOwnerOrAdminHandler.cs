using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using RepositoryTier;
using RepositoryTier.User.Enums;
using System.Security.Claims;

namespace GymManagementAPI.Authorization.SubscriptionOwnerOrAdmin
{
    public class SubscriptionOwnerOrAdminHandler : AuthorizationHandler<SubscriptionOwnerOrAdminRequirement, int>
    {
        private readonly GymManagementDbContext _context;
        public SubscriptionOwnerOrAdminHandler(GymManagementDbContext context)
        {
            _context=context;
        }
        protected override async Task HandleRequirementAsync
            (AuthorizationHandlerContext context, SubscriptionOwnerOrAdminRequirement requirement, int requestSubscriptionId)
        {
            string? tokenUserId = context.User.FindFirstValue(ClaimTypes.NameIdentifier); 
            if (string.IsNullOrEmpty(tokenUserId))
            {
                context.Fail();
                return;
            }
            bool isAdmin = context.User.IsInRole($"{(int)enUserRole.Admin}");
            int userId = Convert.ToInt32(tokenUserId);
            bool isOwnerSubscription = await _context.Subscriptions
                .AnyAsync(s=>
                s.Id== requestSubscriptionId && s.MemberId== userId);
            
            if (isOwnerSubscription || isAdmin)
            {
                context.Succeed(requirement);
            } 

            return;
        }
    }
}
