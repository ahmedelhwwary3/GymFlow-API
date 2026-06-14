using RepositoryTier.Attendance.DTOs;
using RepositoryTier.Attendance.Repositories;
using RepositoryTier.Attendance.Results;
using RepositoryTier.Member.Repositories;
using System.Text;
using System.Threading.Tasks;
using RepositoryTier.Attendance.Enums;
using RepositoryTier.Member.DTOs;
using RepositoryTier.Subscription.Repositories;

namespace ServiceTier.Attendance
{
    public class AttendanceService:Service<RepositoryTier.Entities.Attendance>, IAttendanceService
    {
        private readonly IAttendanceRepository _repo;
        private readonly IMemberRepository _memberRepo; 
        public AttendanceService(
            IAttendanceRepository repo,
            IMemberRepository memberRepo) 
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

        public async Task<AddAttendanceResult> AddAttendancesAsync(AddAttendanceRequest request)
        {
            //1.chech member exists
            bool memberExists=await _memberRepo.ExistsAsync(request.MemberId);
            if (!memberExists)
                return new AddAttendanceResult(enAddAttendanceStatus.MemberNotFound);

            bool hasExpiredSubscription = !await _memberRepo.HasActiveSubscriptionsAsync(request.MemberId);
            if(hasExpiredSubscription) // All Old Subscriptions will be expired as same as last one
                return new AddAttendanceResult(enAddAttendanceStatus.HasExpiredSubscription); 

            bool hasFrozenSubscription = await _memberRepo.HasForzenSubscriptionsAsync(request.MemberId);
            if(hasFrozenSubscription)
                return new AddAttendanceResult(enAddAttendanceStatus.HasFrozenSubscription);

            var attendance = new RepositoryTier.Entities.Attendance()
            {
                AttendanceDate=DateOnly.FromDateTime(DateTime.UtcNow),
                MemberId=request.MemberId,
                Notes=request.Notes?.Trim()
            };

            await _repo.AddAsync(attendance);
            int affectedRows = await _repo.SaveChangesAsync();
            return new AddAttendanceResult(enAddAttendanceStatus.Succeeded,attendance.Id);
        }
    }
}
