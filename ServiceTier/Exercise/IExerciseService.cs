using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceTier.Exercise
{
    public interface IExerciseService: IService<RepositoryTier.Models.Exercise>
    {
    }
}
