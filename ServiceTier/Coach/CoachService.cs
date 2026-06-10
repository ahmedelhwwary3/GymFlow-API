using RepositoryTier.Coach.DTOs;
using RepositoryTier.Coach.Repositories; 
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using models = RepositoryTier.Models;

namespace ServiceTier.Coach
{
    public class CoachService : Service<models.Coach>, ICoachService
    {
        protected readonly ICoachRepository _repo;
        public CoachService(ICoachRepository repo) : base(repo)
        {
            _repo=repo;
        }

        public async Task<GetCoachesResponse> GetCoachesAsync(GetCoachesRequest request)
        {
            return await _repo.GetCoachesAsync(request);
        }
    }
}
