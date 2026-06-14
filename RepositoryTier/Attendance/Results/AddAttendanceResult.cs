using RepositoryTier.Attendance.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.Attendance.Results
{
    public class AddAttendanceResult
    {
        public enAddAttendanceStatus Status { get; set; }
        public int? Id   { get; set; }

        public AddAttendanceResult(enAddAttendanceStatus status,int? Id=null)
        {
            this.Id=Id;
            this.Status=status;
        }
    }
}
