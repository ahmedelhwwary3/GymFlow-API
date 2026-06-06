using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.Data.Repositories.Member
{
    public class MemberRepository : Repository<RepositoryTier.Models.Member>, IMemberRepository
    {
        public MemberRepository(GymManagementDbContext context) : base(context) { }

    }
}
