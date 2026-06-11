using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.Coach.Enums
{
    public enum enAddCoachStatus
    {
        NotUniqueEmail=1,
        NotUniquePhone=2,
        Succeeded=3,
        InternalServerError=4
    }
}
