
using RepositoryTier.Exercise.DTOs;
using RepositoryTier.Exercise.Repositories;
using RepositoryTier.Exercise.Results;
using System;
using System.Collections.Generic;
using RepositoryTier.Exercise.Enums;
using Microsoft.EntityFrameworkCore;
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

        public async Task<bool>IsUniqueName(string name,int Id=0)
        {
            if(Id>0) // Update
            {
                string? exerciseName = await _repo.GetNameAsyncById(Id);
                if (string.IsNullOrEmpty(exerciseName)) // NotFound (Error)
                    return false;

                if (exerciseName == name.Trim())
                    return true;
            }
            return !await _repo.ExistsByNameAsync(name); // Add
        }

        public async Task<AddExerciseResult> AddAsync(AddExerciseRequest request)
        {
            bool isUnique = await IsUniqueName(request.Name);
            if (!isUnique)
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

        public async Task<enUpdateExerciseStatus> UpdateAsync(int Id, UpdateExerciseRequest request)
        {
            //1.Chech uniqueness
            bool isUnique = await IsUniqueName(request.Name, Id);
            if (!isUnique)
                return enUpdateExerciseStatus.NotUniqueName;

            //2.Load then Update strategy
            var exercise = await _repo.FindAsync(Id);
            if (exercise == null)
                return enUpdateExerciseStatus.ExerciseNotFound;
            
            exercise.Description = request.Description;
            exercise.Name = request.Name.Trim(); 

            var state = _repo.GetEntityState(exercise);
            if (state == EntityState.Unchanged)
                return enUpdateExerciseStatus.DataNotChanged;
            exercise.UpdatedAt = DateTime.UtcNow;

            //3. Save
            int affectedRow = await _repo.SaveChangesAsync(); 
            return enUpdateExerciseStatus.Succeeded;
        }
    }
}
