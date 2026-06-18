using Microsoft.AspNetCore.Authorization;

namespace GymManagementAPI.Authorization.OwnerOrAdmin
{
    public class OwnerOrAdminRequirement:IAuthorizationRequirement
    {
    }
}
