using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.Attendance.Enums
{
    public enum enAddAttendanceStatus
    {
        MemberNotFound=1,
        HasExpiredSubscription=2,
        HasFrozenSubscription=3,
        Succeeded=4
    }
}
