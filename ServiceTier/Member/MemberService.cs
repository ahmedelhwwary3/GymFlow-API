 
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
        public MemberService(IMemberRepository repo) : base(repo) { }
    }
}
