using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using models = RepositoryTier.Models;

namespace RepositoryTier.Exercise.Repositories
{
    public class ExerciseRepository : Repository<models.Exercise>, IExerciseRepository
    {
        public ExerciseRepository(GymManagementDbContext context) : base(context) { }
    }
}
