using RepositoryTier.Coach.Repositories; 
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using models = RepositoryTier.Models;

namespace ServiceTier.Coach
{
    public class CoachService : Service<models.Coach>, ICoachService
    {
        public CoachService(ICoachRepository repo) : base(repo) { }
    }
}
