using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.Enums.Users
{
    public enum enChangePasswordResult
    {
        InvalidConfirmPassword,
        UserNotFound,
        InvalidCurrentPassword,
        Succeeded

    }
}
