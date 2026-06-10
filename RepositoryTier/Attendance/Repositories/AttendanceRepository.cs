using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace RepositoryTier.Attendance.Repositories
{
    public class AttendanceRepository : Repository<Entities.Attendance>, IAttendanceRepository
    {
        public AttendanceRepository(GymManagementDbContext context) : base(context) { }


    }
}
