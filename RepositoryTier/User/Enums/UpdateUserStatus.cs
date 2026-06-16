using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.User.Enums
{
    public enum enUpdateUserStatus
    {
        NotUniqueEmail = 1, 
        NotUniquePhone = 2,
        UserNotFound = 3,
        DataNotChanged = 4,
        Succeeded = 5
    }
}
