using RepositoryTier.Member.DTOs; 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepositoryTier.Member.Enums;


namespace ServiceTier.Member
{
    public interface IMemberService 
    {
        Task<GetAssignedMembersForCoachResponse>
            GetAssignedMembersForCoachAsync(int coachId,GetAssignedMembersForCoachRequest request);
        Task<GetMembersResopnse> GetMembersAsync(GetMembersRequest request);
        
        Task<enUpdateMemberStatus> UpdateAsync(int Id,UpdateMemberRequest request);
        Task<enUpdateMemberProfileStatus> UpdateProfileAsync(int Id, UpdateMemberProfileRequest request);
        Task<GetMemberProfileResopnse?> GetProfileByIdAsync(int Id);
        Task<GetMemberByIdResopnse?> GetByIdAsync(int Id);
    }
}
