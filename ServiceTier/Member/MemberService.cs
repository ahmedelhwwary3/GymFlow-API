using RepositoryTier.Data.Repositories;
using RepositoryTier.Data.Repositories.Member;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceTier.Member
{
    public class MemberService: Service<RepositoryTier.Models.Member>, IMemberService
    {
        public MemberService(IMemberRepository repo) : base(repo) { }
    }
}
