using Microsoft.AspNetCore.Mvc;
using RepositoryTier.Entities;
using System.Security.Claims; 

namespace GymManagementAPI.Extensions
{
    public static class HttpContextExtensions
    {
        

        public static string GetIPAddress(
            this HttpContext context)
        {
            return context.Connection.RemoteIpAddress?
                .ToString() ?? "Anonymous";
        }

        public static string GetAdminId(
            this HttpContext context)
        {
            return context.User
                .FindFirstValue(ClaimTypes.NameIdentifier) ?? "Anonymous";
        }

        
    }
}
