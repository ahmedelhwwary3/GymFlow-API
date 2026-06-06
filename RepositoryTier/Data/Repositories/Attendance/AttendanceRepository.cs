using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.Data.Repositories.Attendance
{
    public class AttendanceRepository : Repository<RepositoryTier.Models.Attendance>, IAttendanceRepository
    {
        public AttendanceRepository(GymManagementDbContext context) : base(context) { }


    }
}
