using RepositoryTier.Attendance.Repositories; 
using System.Text;
using System.Threading.Tasks; 

namespace ServiceTier.Attendance
{
    public class AttendanceService:Service<RepositoryTier.Entities.Attendance>, IAttendanceService
    {
        public AttendanceService(IAttendanceRepository repo) : base(repo) { }
    }
}
