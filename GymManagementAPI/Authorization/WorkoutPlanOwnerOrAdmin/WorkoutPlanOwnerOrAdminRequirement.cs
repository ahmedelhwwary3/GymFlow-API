using Microsoft.AspNetCore.Authorization;

namespace GymManagementAPI.Authorization.WorkoutPlanOwnerOrAdmin
{
    public class WorkoutPlanOwnerOrAdminRequirement:IAuthorizationRequirement
    {
    }
}
