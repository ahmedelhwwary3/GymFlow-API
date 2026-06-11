using RepositoryTier.Coach.DTOs;
using RepositoryTier.Coach.Enums; 
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
        Task<AddCoachResponse> AddAsync(AddCoachRequest request);
        Task<GetCoachByIdResponse?> GetByIdAsync(int Id);
        Task<enUpdateCoachByIdStatus> UpdateByIdAsync(int Id,UpdateCoachByIdRequest request);
    }
}
