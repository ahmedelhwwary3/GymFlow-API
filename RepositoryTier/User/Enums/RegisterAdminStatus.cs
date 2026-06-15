using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.User.Enums
{
    public enum enRegisterAdminStatus
    {
        NotUniqueEmail = 1,
        NotUniquePhone = 2,
        Succeeded = 3
    }
}
