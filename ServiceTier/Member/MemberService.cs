using RepositoryTier.Data.Repositories;
using RepositoryTier.Data.Repositories.Member;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using models = RepositoryTier.Models;

namespace ServiceTier.Member
{
    public class MemberService: Service<models.Member>, IMemberService
    {
        public MemberService(IMemberRepository repo) : base(repo) { }
    }
}
