using RepositoryTier.Exercise.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.Exercise.DTOs
{
    public class UpdateExerciseRequest
    {
        [Required]
        public string Name { get; set; }

        public string? Description { get; set; }

        [EnumDataType(typeof(enTargetMuscleGroup))]
        public enTargetMuscleGroup TargetMuscleGroup { get; set; }
    }
}
