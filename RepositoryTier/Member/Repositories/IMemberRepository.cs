using RepositoryTier.Member.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace RepositoryTier.Member.Repositories
{
    public interface IMemberRepository : IRepository<Entities.Member>
    {
        Task<GetAssignedMembersForCoachResponse>
            GetAssignedMembersForCoachAsync(int coachId, GetAssignedMembersForCoachRequest request);
        Task<GetMembersResopnse> GetMembersAsync(GetMembersRequest request); 
        Task<GetMemberProfileResopnse?> GetProfileAsync(int Id);
        Task<GetMemberByIdResopnse?> GetByIdAsync(int Id);
        Task<Boolean> HasActiveSubscriptionsAsync(int memberId); 
        Task<Boolean> HasForzenSubscriptionsAsync(int memberId);
        Task<int?> GetIdByIdentifierAsync(string identifier);
        Task<Boolean> HasAttendanceInDateAsync(int memberId,DateOnly date);
    }
}
