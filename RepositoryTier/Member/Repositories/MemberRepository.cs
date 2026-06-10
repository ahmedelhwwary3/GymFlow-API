using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace RepositoryTier.Member.Repositories
{
    public class MemberRepository : Repository<Entities.Member>, IMemberRepository
    {
        public MemberRepository(GymManagementDbContext context) : base(context) { }

    }
}
