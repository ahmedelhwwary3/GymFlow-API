using RepositoryTier.Exercise.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.Exercise.DTOs
{
    public class GetExercisesResponse
    {
        public List<ExerciseResponse> Exercises {  get; set; }
        public int Count { get; set; }
    }
}
