using RepositoryTier.Exercise.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.Exercise.DTOs
{
    public class ExerciseResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public enTargetMuscleGroup TargetMuscleGroup { get; set; }
        public string? Description { get; set; }
    }
}
