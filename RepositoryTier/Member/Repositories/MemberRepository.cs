using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using RepositoryTier.API_Configurations;
using RepositoryTier.Member.DTOs;
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

        public async Task<GetAssignedMembersForCoachResponse> 
            GetAssignedMembersForCoachAsync(GetAssignedMembersForCoachRequest request)
        {
            int page= PaganationOptions.Page;
            int pageSize= PaganationOptions.BigPageSize;

            int totalCount =await _context.Members
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
    }
}
