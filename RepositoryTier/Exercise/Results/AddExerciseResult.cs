using RepositoryTier.Exercise.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.Exercise.Results
{
    public class AddExerciseResult
    {
        public enAddExerciseStatus Status { get; set; }
        public int? Id { get; set; }
        public AddExerciseResult(enAddExerciseStatus status,int? Id=null)
        {
            this.Id=Id;
            this.Status= status;
        }
    }
}
