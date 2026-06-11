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
    }
}
