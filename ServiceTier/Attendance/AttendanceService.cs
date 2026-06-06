using RepositoryTier.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceTier.Attendance
{
    public class AttendanceService:Service<RepositoryTier.Models.Attendance>, IAttendanceService
    {
        public AttendanceService(IRepository<RepositoryTier.Models.Attendance> repo) : base(repo) { }
    }
}
