using RepositoryTier.Exercise.DTOs;
using RepositoryTier.Exercise.Enums;
using RepositoryTier.Exercise.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceTier.Exercise
{
    public interface IExerciseService: IService<RepositoryTier.Entities.Exercise>
    {
        Task<GetExercisesResponse> GetExercisesAsync(GetExercisesRequest request);
        Task<AddExerciseResult> AddAsync(AddExerciseRequest request);
        Task<enUpdateExerciseStatus> UpdateByIdAsync(int Id,UpdateExerciseRequest request); 
    }
}
