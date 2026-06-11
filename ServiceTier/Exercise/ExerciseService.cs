 
using RepositoryTier.Exercise.Repositories;
using System;
using System.Collections.Generic;
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

    }
}
