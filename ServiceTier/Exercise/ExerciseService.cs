
using RepositoryTier.Exercise.DTOs;
using RepositoryTier.Exercise.Repositories;
using RepositoryTier.Exercise.Results;
using System;
using System.Collections.Generic;
using RepositoryTier.Exercise.Enums;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace ServiceTier.Exercise
{
    public class ExerciseService : Service<RepositoryTier.Entities.Exercise>, IExerciseService
    {
        private readonly IExerciseRepository _repo;
        public ExerciseService(IExerciseRepository repo) : base(repo) 
        { 
            _repo = repo;
        }

        public async Task<GetExercisesResponse> GetExercisesAsync(GetExercisesRequest request)
        {
            return await _repo.GetExercisesAsync(request);
        }

        public async Task<AddExerciseResult> AddAsync(AddExerciseRequest request)
        {
            bool exists = await _repo.ExistsByNameAsync(request.Name.Trim());
            if (exists)
                return new AddExerciseResult(enAddExerciseStatus.NotUniqueName);

            var newExercise = new RepositoryTier.Entities.Exercise()
            {
                Name = request.Name.Trim(),
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false,
                Description=request.Description,
                TargetMuscleGroup=request.TargetMuscleGroup
            };

            await _repo.AddAsync(newExercise);
            await _repo.SaveChangesAsync();
            return new AddExerciseResult(enAddExerciseStatus.Succeeded, newExercise.Id);
        }
    }
}
