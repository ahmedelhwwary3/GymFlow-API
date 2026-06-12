using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.Member.Enums
{
    public enum enUpdateMemberStatus
    {
        NotUniqueEmail = 1,
        NotUniquePhone = 2,
        CoachNotExists = 3,
        CoachInactive = 4,
        MemberNotFound= 5,
        DataNotChanged= 6,
        InternalServerError= 7,
        Succeeded = 8
    }
    public enum enUpdateMemberProfileStatus
    {
        NotUniqueEmail = 1,
        NotUniquePhone = 2, 
        MemberNotFound = 3,
        DataNotChanged = 4,
        InternalServerError = 5,
        Succeeded = 6
    }
}
