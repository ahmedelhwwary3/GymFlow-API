using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.User.Enums
{
    public enum enRegisterMemberStatus
    {
        NotUniqueEmail = 1,
        NotUniquePhone = 2, 
        CoachNotExists = 3, 
        CoachInactive = 4, 
        Succeeded = 5
    }
}
