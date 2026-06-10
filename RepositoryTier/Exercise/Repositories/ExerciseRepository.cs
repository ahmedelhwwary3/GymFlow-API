using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using RepositoryTier.Entities;

namespace RepositoryTier.Exercise.Repositories
{
    public class ExerciseRepository : Repository<Entities.Exercise>, IExerciseRepository
    {
        public ExerciseRepository(GymManagementDbContext context) : base(context) { }
    }
}
