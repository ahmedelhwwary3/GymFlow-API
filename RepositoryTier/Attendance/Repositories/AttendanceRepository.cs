using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RepositoryTier.API_Configurations;
using RepositoryTier.Attendance.DTOs;
using RepositoryTier.Coach.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace RepositoryTier.Attendance.Repositories
{
    public class AttendanceRepository : Repository<Entities.Attendance>, IAttendanceRepository
    {
        protected readonly PaganationOptions _paganationOptions;
        public AttendanceRepository(
            GymManagementDbContext context,
            IOptions<PaganationOptions>paganationOptions
            
            ) 
            : base(context) 
        {
            _paganationOptions = paganationOptions.Value;
            if (_paganationOptions == null)
                throw new Exception("Paganation options is not configured");
        }

        public async Task<GetAttendancesResponse>
            GetAttendancesAsync(GetAttendancesRequest request, int? memberId = null)
        {
            List<CoachLookUpResponse> coaches = default;
            int count;
            int page = request.Page ?? _paganationOptions.Page;
            int pageSize = request.PageSize ?? _paganationOptions.TinyPageSize;

            var query = _context.Attendances //1.Both Filtering
                .IgnoreQueryFilters()
                .AsNoTracking()
                .Where(a =>
                (request.FromDate == null || a.AttendanceDate >= request.FromDate) &&
                (request.ToDate == null || a.AttendanceDate <= request.ToDate));

            if (request.CoachId.HasValue)
                query = query.Where(a => a.Member.CoachId == request.CoachId);

            if (memberId.HasValue) // 2.Member Filtering
                query = query.Where(a => a.MemberId == memberId);
            else // 3.Admin Filtering
            {
                query = query.Where(a => string.IsNullOrEmpty(request.Search)
                || a.Member.FullName.Contains(request.Search.Trim())
                || a.Member.Phone.Contains(request.Search.Trim())
                || a.Member.Email.Contains(request.Search.Trim()));

                coaches = await _context.Coaches.Select(c =>
                new CoachLookUpResponse()
                {
                    FullName = c.FullName,
                    Id = c.Id
                }).ToListAsync();
            }
            // 4.Count before paganation after filtering - for member or admin 
            count = await query.CountAsync();
            // 5.Paganation
            query = query
                .OrderByDescending(a => a.AttendanceDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
            // 6.Projection then Execusion
            var attendances = await query.Select(a => new AttendanceResponse()
            {
                AttendanceDate = a.AttendanceDate,
                AttendanceId = a.Id,
                CoachId = a.Member.CoachId.Value,
                CoachName = a.Member.Coach.FullName,
                MemberName = a.Member.FullName
            }).ToListAsync();

            return new GetAttendancesResponse()
            {
                Attendances = attendances,
                Coaches = coaches,
                Count = count
            };
        }
    }
}
