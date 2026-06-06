using RepositoryTier.Data.Repositories;
using RepositoryTier.Data.Repositories.Attendance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceTier.Attendance
{
    public class AttendanceService:Service<RepositoryTier.Models.Attendance>, IAttendanceService
    {
        public AttendanceService(IAttendanceRepository repo) : base(repo) { }
    }
}
