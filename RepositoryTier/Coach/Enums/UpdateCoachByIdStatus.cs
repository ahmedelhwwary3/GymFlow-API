using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.Coach.Enums
{
    public enum enUpdateCoachStatus
    {
        CoachNotFound=1,
        Succeeded=2,
        NotUniquePhone=3,
        NotUniqueEmail=4,
        DataNotChanged=5,
        InternalServerError=6
    }
}
