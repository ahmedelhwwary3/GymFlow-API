using RepositoryTier.Attendance.DTOs;
using RepositoryTier.Attendance.Repositories;
using RepositoryTier.Attendance.Results;
using RepositoryTier.Member.Repositories;
using System.Text;
using System.Threading.Tasks;
using RepositoryTier.Attendance.Enums;
using RepositoryTier.Member.DTOs;
using RepositoryTier.Subscription.Repositories;
using RepositoryTier.User.Repositories;
using RepositoryTier.Entities;

namespace ServiceTier.Attendance
{
    public class AttendanceService:Service<RepositoryTier.Entities.Attendance>, IAttendanceService
    {
        private readonly IAttendanceRepository _repo;
        private readonly IMemberRepository _memberRepo; 
        public AttendanceService(
            IAttendanceRepository repo,
            IMemberRepository memberRepo ) 
            : base(repo)
        { 
            _repo = repo;
            _memberRepo = memberRepo; 
        }

        public async Task<GetAttendancesResponse>
            GetAttendancesAsync(GetAttendancesRequest request, int? memberId = null)
        {
            return await _repo.GetAttendancesAsync(request,memberId);
        }

        public async Task<AddAttendanceResult> AddAttendanceAsync(AddAttendanceRequest request)
        {
            //1.chech member exists
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            int? Id;
            // using Id
            if (request.MemberId.HasValue)
            {
                bool exists = await _memberRepo.ExistsAsync(request.MemberId.Value);
                if (!exists)
                    return new AddAttendanceResult(enAddAttendanceStatus.MemberNotFound);
                Id = request.MemberId;
            }
            else // using phone or email
            {
                Id = await _memberRepo.GetIdByIdentifierAsync(request.Search.Trim());
                if (Id == null)
                    return new AddAttendanceResult(enAddAttendanceStatus.MemberNotFound);
            }

            bool hasExpiredSubscription = !await _memberRepo.HasActiveSubscriptionsAsync(Id.Value);
            if (hasExpiredSubscription) // All Old Subscriptions will be expired as same as last one
                return new AddAttendanceResult(enAddAttendanceStatus.HasExpiredSubscription);

            bool hasFrozenSubscription = await _memberRepo.HasForzenSubscriptionsAsync(Id.Value);
            if (hasFrozenSubscription)
                return new AddAttendanceResult(enAddAttendanceStatus.HasFrozenSubscription);

            bool hasTodayAttendance = await _memberRepo.HasAttendanceInDateAsync(Id.Value,today);
            if(hasTodayAttendance)
                return new AddAttendanceResult(enAddAttendanceStatus.HasTodayAttendance);

            var attendance = new RepositoryTier.Entities.Attendance()
            {
                AttendanceDate = today,
                MemberId = Id.Value,
                Notes = request.Notes?.Trim()
            };

            await _repo.AddAsync(attendance);
            int affectedRows = await _repo.SaveChangesAsync();
            return new AddAttendanceResult(enAddAttendanceStatus.Succeeded, attendance.Id);
        }
    }
}
