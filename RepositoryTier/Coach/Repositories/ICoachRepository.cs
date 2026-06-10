using RepositoryTier.Coach.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace RepositoryTier.Coach.Repositories
{
    public interface ICoachRepository: IRepository<Entities.Coach>
    {
        Task<GetCoachesResponse> GetCoachesAsync(GetCoachesRequest request);
    }
}
