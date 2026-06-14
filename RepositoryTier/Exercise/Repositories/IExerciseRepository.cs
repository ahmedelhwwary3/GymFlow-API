using RepositoryTier.Exercise.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace RepositoryTier.Exercise.Repositories
{
    public interface IExerciseRepository:IRepository<Entities.Exercise>
    {
        Task<GetExercisesResponse> GetExercisesAsync(GetExercisesRequest request);
        Task<Boolean> ExistsByNameAsync(string name);
    }
}
