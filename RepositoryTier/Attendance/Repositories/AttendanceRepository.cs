using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using models = RepositoryTier.Models;

namespace RepositoryTier.Attendance.Repositories
{
    public class AttendanceRepository : Repository<models.Attendance>, IAttendanceRepository
    {
        public AttendanceRepository(GymManagementDbContext context) : base(context) { }


    }
}
