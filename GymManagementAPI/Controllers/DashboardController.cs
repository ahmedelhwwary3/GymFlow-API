using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryTier.User.Enums;
using ServiceTier.Dashboard;
using System.Security.Claims;

namespace GymManagementAPI.Controllers
{
    [Route("api/Dashboard")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;
        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService=dashboardService;
        }

        [HttpGet(Name = "GetDashboard")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)] 
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetDashboard()
        { 
            bool isAdmin = User.IsInRole(((int)enUserRole.Admin).ToString());
            bool isCoach = User.IsInRole(((int)enUserRole.Coach).ToString());
            bool isMember = User.IsInRole(((int)enUserRole.Member).ToString());

            if(isAdmin)
            {
                var response = await _dashboardService.GetForAdminAsync();
                return Ok(response);
            }

            // Member or Coach (get Id first)
            string? userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Token is no longer valid");
            int Id = Convert.ToInt32(userId);

            if (isCoach)
            {
                var response = await _dashboardService.GetForCoachAsync(Id);
                return Ok(response);
            }
            else
            {
                var response = await _dashboardService.GetForMemberAsync(Id);
                return response == null ? NotFound("Member is not found") : Ok(response);
            }
        }
    }
}
