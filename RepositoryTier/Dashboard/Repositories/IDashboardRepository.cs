using RepositoryTier.Dashboard.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace RepositoryTier.Dashboard.Repositories
{
    public interface IDashboardRepository 
    {
        Task<AdminDashboardResponse> GetForAdminAsync();
        Task<CoachDashboardResponse> GetForCoachAsync(int coachId);
        Task<MemberDashboardResponse?> GetForMemberAsync(int memberId);
    }
}
