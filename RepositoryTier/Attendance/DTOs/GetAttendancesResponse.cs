using RepositoryTier.Coach.DTOs;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.Attendance.DTOs
{
    public class GetAttendancesResponse
    {
        public List<AttendanceResponse> Attendances { get; set; }
        public List<CoachLookUpResponse>? Coaches { get; set; }
        public int Count { get; set; }
    }
}
