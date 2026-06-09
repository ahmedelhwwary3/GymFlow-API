using RepositoryTier.Attendance.Repositories; 
using System.Text;
using System.Threading.Tasks;
using models = RepositoryTier.Models;

namespace ServiceTier.Attendance
{
    public class AttendanceService:Service<models.Attendance>, IAttendanceService
    {
        public AttendanceService(IAttendanceRepository repo) : base(repo) { }
    }
}
