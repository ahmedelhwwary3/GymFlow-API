using RepositoryTier.Coach.DTOs;
using RepositoryTier.Coach.Enums;
using RepositoryTier.Coach.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace ServiceTier.Coach
{
    public interface ICoachService:IService<RepositoryTier.Entities.Coach>
    {
        Task<GetCoachesResponse> GetCoachesAsync(GetCoachesRequest request);
        Task<AddCoachResult> AddAsync(AddCoachRequest request);
        Task<GetCoachByIdResponse?> GetByIdAsync(int Id);
        Task<enUpdateCoachByIdStatus> UpdateByIdAsync(int Id,UpdateCoachByIdRequest request);
        Task<List<CoachLookUpResponse>> GetLookUpCoachesAsync();
    }
}
