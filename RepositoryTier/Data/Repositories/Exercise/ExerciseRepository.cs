using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.Data.Repositories.Exercise
{
    public class ExerciseRepository : Repository<RepositoryTier.Models.Exercise>, IExerciseRepository
    {
        public ExerciseRepository(GymManagementDbContext context) : base(context) { }
    }
}
