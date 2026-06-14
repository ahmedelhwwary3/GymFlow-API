using RepositoryTier.Attendance.DTOs;
using RepositoryTier.Attendance.Repositories; 
using System.Text;
using System.Threading.Tasks; 

namespace ServiceTier.Attendance
{
    public class AttendanceService:Service<RepositoryTier.Entities.Attendance>, IAttendanceService
    {
        private readonly IAttendanceRepository _repo;
        public AttendanceService(IAttendanceRepository repo) : base(repo)
        { 
            _repo = repo;
        }

        public async Task<GetAttendancesResponse>
            GetAttendancesAsync(GetAttendancesRequest request, int? memberId = null)
        {
            return await _repo.GetAttendancesAsync(request,memberId);
        }
    }
}
