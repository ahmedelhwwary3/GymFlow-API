using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.User.Enums
{
    public enum enChangePasswordStatus
    {
        InvalidConfirmPassword,
        UserNotFound,
        InvalidCurrentPassword,
        Succeeded,
        OldPassword

    }
}
