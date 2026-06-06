using RepositoryTier.Data.Repositories;
using RepositoryTier.Data.Repositories.Exercise;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceTier.Exercise
{
    public class ExerciseService : Service<RepositoryTier.Models.Exercise>, IExerciseService
    {
        public ExerciseService(IExerciseRepository repo) : base(repo) { }

    }
}
