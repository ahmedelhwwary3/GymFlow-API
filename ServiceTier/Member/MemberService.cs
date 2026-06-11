
using RepositoryTier.Member.DTOs;
using RepositoryTier.Member.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace ServiceTier.Member
{
    public class MemberService: Service<RepositoryTier.Entities.Member>, IMemberService
    {
        private readonly IMemberRepository _repo;
        public MemberService(IMemberRepository repo) : base(repo) 
        {
            _repo = repo;
        }


        public async Task<GetAssignedMembersForCoachResponse>
            GetAssignedMembersForCoachAsync(GetAssignedMembersForCoachRequest request)
        {
            return await _repo.GetAssignedMembersForCoachAsync(request);
        }
    }
}
