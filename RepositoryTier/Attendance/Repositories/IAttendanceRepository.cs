using RepositoryTier.Attendance.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace RepositoryTier.Attendance.Repositories
{
    public interface IAttendanceRepository:IRepository<Entities.Attendance>
    {
        Task<GetAttendancesResponse> GetAttendancesAsync(GetAttendancesRequest request, int? memberId = null);
    }
}
