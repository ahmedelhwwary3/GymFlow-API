using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RepositoryTier.Entities;
using RepositoryTier.Exercise.DTOs;

namespace RepositoryTier.Exercise.Repositories
{
    public class ExerciseRepository : Repository<Entities.Exercise>, IExerciseRepository
    {
        public ExerciseRepository(GymManagementDbContext context) : base(context) { }

        public async Task<GetExercisesResponse> GetExercisesAsync(GetExercisesRequest request)
        {
            var query = _context.Exercises
                .AsNoTracking()
                .Where(e =>
                (request.TargetMuscleGroup == null || e.TargetMuscleGroup == request.TargetMuscleGroup) &&
                (string.IsNullOrEmpty(request.Search) || e.Name.Contains(request.Search.Trim())));

            var exercises = await query
                .Select(e => new ExerciseResponse()
                {
                    Id = e.Id,
                    Name = e.Name,
                    Description = e.Description,
                    TargetMuscleGroup = e.TargetMuscleGroup
                }).ToListAsync();

            int count = await _context.Exercises.CountAsync();
            return new GetExercisesResponse()
            {
                Count= count,
                Exercises= exercises
            };
        }
    }
}
