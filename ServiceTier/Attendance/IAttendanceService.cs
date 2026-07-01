using RepositoryTier.Attendance.DTOs;
using RepositoryTier.Attendance.Results;
using RepositoryTier.Member.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace ServiceTier.Attendance
{
    public interface IAttendanceService 
    {
        Task<GetAttendancesResponse> GetAttendancesAsync(GetAttendancesRequest request, int? memberId = null);
        Task<AddAttendanceResult> AddAttendanceAsync(AddAttendanceRequest memberId);
    }
}
