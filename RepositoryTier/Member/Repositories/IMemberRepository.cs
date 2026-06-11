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
            GetAssignedMembersForCoachAsync(GetAssignedMembersForCoachRequest request);
        Task<GetMembersResopnse> GetMembersAsync(GetMembersRequest request);
    }
}
