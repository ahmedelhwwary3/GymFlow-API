using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace RepositoryTier.Dashboard.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly GymManagementDbContext _context;
        public DashboardRepository(GymManagementDbContext context)
        {
            _context = context;
        }


    }
}
