using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.Member.Enums
{
    public enum enAddMemberStatus
    {
        NotUniqueEmail = 1,
        NotUniquePhone = 2, 
        CoachNotExists = 3, 
        CoachInactive = 4, 
        Succeeded = 5, 
        InternalServerError = 6
    }
}
