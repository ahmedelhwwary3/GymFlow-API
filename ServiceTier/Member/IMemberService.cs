using RepositoryTier.Member.DTOs;
using RepositoryTier.Member.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepositoryTier.Member.Enums;


namespace ServiceTier.Member
{
    public interface IMemberService:IService<RepositoryTier.Entities.Member>
    {
        Task<GetAssignedMembersForCoachResponse>
            GetAssignedMembersForCoachAsync(GetAssignedMembersForCoachRequest request);
        Task<GetMembersResopnse> GetMembersAsync(GetMembersRequest request);
        Task<AddMemberResult> AddAsync(AddMemberRequest request);
        Task<enUpdateMemberStatus> UpdateAsync(int Id,UpdateMemberRequest request);
        Task<enUpdateMemberProfileStatus> UpdateProfileAsync(int Id, UpdateMemberProfileRequest request);
        Task<GetMemberProfileResopnse?> GetProfileAsync(int Id);
        Task<GetMemberByIdResopnse?> GetMemberByIdAsync(int Id);
    }
}
