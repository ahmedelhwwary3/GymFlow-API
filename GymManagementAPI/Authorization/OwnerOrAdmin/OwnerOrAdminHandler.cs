using Microsoft.AspNetCore.Authorization;
using RepositoryTier.User.Enums;
using System.Security.Claims;

namespace GymManagementAPI.Authorization.OwnerOrAdmin
{
    public class OwnerOrAdminHandler : AuthorizationHandler<OwnerOrAdminRequirement, int> // userId is resource
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context, OwnerOrAdminRequirement requirement, int requestUserId)
        {
            string? tokenUserId = context.User.FindFirstValue(ClaimTypes.NameIdentifier); 
            if(string.IsNullOrEmpty(tokenUserId))
            {
                context.Fail();
                return Task.CompletedTask;
            }

            bool isAdmin = context.User.IsInRole($"{(int)enUserRole.Admin}");
            int userId=Convert.ToInt32(tokenUserId);

            if (userId == requestUserId || isAdmin)
            {
                context.Succeed(requirement);
            }
             
            return Task.CompletedTask;
        }
    }
}
