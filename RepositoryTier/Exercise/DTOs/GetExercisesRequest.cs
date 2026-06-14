using RepositoryTier.Exercise.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.Exercise.DTOs
{
    public class GetExercisesRequest
    {
        [EnumDataType(typeof(enTargetMuscleGroup))]
        public enTargetMuscleGroup? TargetMuscleGroup { get; set; }
        public string? Search {  get; set; }
    }
}
