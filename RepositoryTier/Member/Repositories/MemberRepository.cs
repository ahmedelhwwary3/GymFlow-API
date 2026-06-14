using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RepositoryTier.API_Configurations;
using RepositoryTier.Coach.DTOs;
using RepositoryTier.Coach.Enums;
using RepositoryTier.Entities;
using RepositoryTier.Member.DTOs;
using RepositoryTier.Member.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.Member.Repositories
{
    public class MemberRepository : Repository<Entities.Member>, IMemberRepository
    {
        protected readonly PaganationOptions PaganationOptions;
        public MemberRepository(
            GymManagementDbContext context,
            IOptions<PaganationOptions>paganationOptions) : base(context) 
        {
            PaganationOptions = paganationOptions.Value;
            if (PaganationOptions == null)
                throw new Exception("Paganation options is not configured");
        }

        private async Task<List<CoachLookUpResponse>>GetLookupCoachesAsync()
        {
            return await _context.Coaches
                .AsNoTracking()
                .Select(c =>
            new CoachLookUpResponse()
            {
                FullName = c.FullName,
                Id = c.Id
            }).ToListAsync();
        }

        public async Task<GetAssignedMembersForCoachResponse> 
            GetAssignedMembersForCoachAsync(GetAssignedMembersForCoachRequest request)
        {
            int page= request.Page?? PaganationOptions.Page;
            int pageSize=request.PageSize?? PaganationOptions.BigPageSize;

            int totalCount =await _context.Members
                .IgnoreQueryFilters()
                .Where(c => c.CoachId == request.CoachId)
                .CountAsync();

            var members = await _context.Members
               .Where(m => m.CoachId == request.CoachId)
               .Skip((page - 1) * pageSize)
               .Take(pageSize)
               .Select(m => new GetAssignedMemberForCoachResponse()
               {
                   FullName = m.FullName,
                   Id = m.Id,
                   Phone = m.Phone
               }).ToListAsync();

            return new GetAssignedMembersForCoachResponse()
            {
                Members = members,
                Count = totalCount
            };
        }

        public async Task<GetMembersResopnse>
            GetMembersAsync(GetMembersRequest request)
        {
            int page = request.Page ?? PaganationOptions.Page;
            int pageSize = request.PageSize ?? PaganationOptions.BigPageSize;

            int totalCount = await _context.Members.CountAsync();

            var query = _context.Members
               .IgnoreQueryFilters()
               .AsNoTracking()
               .Where(m => (request.CoachId == null || m.CoachId == request.CoachId) &&
               (string.IsNullOrEmpty(request.Search) || m.Phone.Contains(request.Search)
               || m.Email.Contains(request.Search) || m.FullName.Contains(request.Search)) &&
               (request.IsActive == null || m.IsActive == request.IsActive) &&
               (request.Gender == null || m.Gender == request.Gender));

            query = request.Sort switch
            {
                enMemberSort.FullName => query.OrderBy(c => c.FullName),
                enMemberSort.CoachName => query.OrderBy(c => c.Coach.FullName),
                _ => query.OrderBy(c => c.CreatedAt)
            };

            var members = await query
               .Skip((page - 1) * pageSize)
               .Take(pageSize)
               .Select(m => new MemberResponse()
               {
                   FullName = m.FullName,
                   Id = m.Id,
                   Phone = m.Phone,
                   CoachName = m.Coach.FullName,
                   IsActive = m.IsActive,
                   Email = m.Email,
                   CoachId = m.CoachId
               }).ToListAsync();

            var coaches =await GetLookupCoachesAsync();

            return new GetMembersResopnse()
            {
                Members = members,
                Count = totalCount,
                Coaches = coaches
            };
        }

        public async Task<bool>HasActiveSubscriptionAsync(int Id)
        {
            return await _context.Subscriptions
                .AnyAsync(s => s.MemberId == Id &&
                s.EndDate < DateOnly.FromDateTime(DateTime.UtcNow));
        }

        public async Task<GetMemberByIdResopnse?> GetMemberByIdAsync(int Id)
        {
            var coaches = await GetLookupCoachesAsync();

            return await _context.Members
            .AsNoTracking()
            .Select(m => new GetMemberByIdResopnse()
            {
                Id = m.Id,
                Address= m.Address,
                CoachId=m.CoachId, 
                DateOfBirth=m.DateOfBirth,
                Email=m.Email,
                FitnessGoal=m.FitnessGoal,
                FullName=m.FullName,
                Gender=m.Gender,
                Height=m.Height,
                Phone=m.Phone, 
                Coaches=coaches
            }).FirstOrDefaultAsync(m => m.Id == Id);
        }

        public async Task<GetMemberProfileResopnse?> GetProfileAsync(int Id)
        {
            return await _context.Members
            .IgnoreQueryFilters()
            .AsNoTracking()
            .Where(m => m.Id == Id)
            .Select(m => new GetMemberProfileResopnse()
            {
                Id=m.Id,
                Address = m.Address,
                Email = m.Email,
                FitnessGoal = m.FitnessGoal,
                Height = m.Height,
                Phone = m.Phone,
                CoachName = m.Coach.FullName
            }).FirstOrDefaultAsync();
        }

        public async Task<bool> HasActiveSubscriptionsAsync(int Id) 
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            return await _context.Subscriptions
                .AnyAsync(s => (s.MemberId == Id) &&
                (s.FreezeEndDate == null && s.EndDate > today)); 

        }

        public async Task<bool> HasForzenSubscriptionsAsync(int Id)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            return await _context.Subscriptions
                .AnyAsync(s => (s.MemberId == Id) &&
                (s.FreezeEndDate != null && s.FreezeEndDate > today)); 
        }
    }
}
