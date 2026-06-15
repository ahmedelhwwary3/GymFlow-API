using RepositoryTier.Dashboard.DTOs;
using RepositoryTier.Dashboard.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceTier.Dashboard
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _repo;

        public DashboardService(IDashboardRepository repo)
        {
            _repo= repo;
        }

        public async Task<AdminDashboardResponse> GetForAdminAsync()
        {
            return await _repo.GetForAdminAsync();
        }

        public async Task<CoachDashboardResponse> GetForCoachAsync(int coachId)
        {
            return await _repo.GetForCoachAsync(coachId);
        }

        public async Task<MemberDashboardResponse?> GetForMemberAsync(int memberId)
        {
            return await _repo.GetForMemberAsync(memberId);
        }
    }
}
