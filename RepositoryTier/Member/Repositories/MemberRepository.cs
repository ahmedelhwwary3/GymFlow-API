using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using models = RepositoryTier.Models;

namespace RepositoryTier.Member.Repositories
{
    public class MemberRepository : Repository<models.Member>, IMemberRepository
    {
        public MemberRepository(GymManagementDbContext context) : base(context) { }

    }
}
