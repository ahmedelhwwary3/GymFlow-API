using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.Data.Repositories.Attendance
{
    internal class AttendanceRepository : Repository<GymManagementAPI.Models.Attendance>, IAttendanceRepository
    {
        public AttendanceRepository(GymManagementDbContext context) : base(context) { }


    }
}
