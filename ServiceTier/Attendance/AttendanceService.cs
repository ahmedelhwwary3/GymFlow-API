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
        private readonly IUserRepository _userRepo;
        public AttendanceService(
            IAttendanceRepository repo,
            IMemberRepository memberRepo,
            IUserRepository userRepo) 
            : base(repo)
        { 
            _repo = repo;
            _memberRepo = memberRepo;
            _userRepo = userRepo;
        }

        public async Task<GetAttendancesResponse>
            GetAttendancesAsync(GetAttendancesRequest request, int? memberId = null)
        {
            return await _repo.GetAttendancesAsync(request,memberId);
        }

        public async Task<AddAttendanceResult> AddAttendancesAsync(AddAttendanceRequest request)
        {
            //1.chech member exists
            int? Id;
            // using Id
            if (request.Id.HasValue)
            {
                bool exists = await _userRepo.ExistsAsync(request.Id.Value);
                if (!exists)
                    return new AddAttendanceResult(enAddAttendanceStatus.MemberNotFound);
                Id = request.Id;
            }
            else // using phone or email
            {
                Id = await _userRepo.GetIdByIdentifierAsync(request.Search.Trim());
                if (Id == null)
                    return new AddAttendanceResult(enAddAttendanceStatus.MemberNotFound);
            }

            bool hasExpiredSubscription = !await _memberRepo.HasActiveSubscriptionsAsync(Id.Value);
            if (hasExpiredSubscription) // All Old Subscriptions will be expired as same as last one
                return new AddAttendanceResult(enAddAttendanceStatus.HasExpiredSubscription);

            bool hasFrozenSubscription = await _memberRepo.HasForzenSubscriptionsAsync(Id.Value);
            if (hasFrozenSubscription)
                return new AddAttendanceResult(enAddAttendanceStatus.HasFrozenSubscription);

            var attendance = new RepositoryTier.Entities.Attendance()
            {
                AttendanceDate = DateOnly.FromDateTime(DateTime.UtcNow),
                MemberId = Id.Value,
                Notes = request.Notes?.Trim()
            };

            await _repo.AddAsync(attendance);
            int affectedRows = await _repo.SaveChangesAsync();
            return new AddAttendanceResult(enAddAttendanceStatus.Succeeded, attendance.Id);
        }
    }
}
