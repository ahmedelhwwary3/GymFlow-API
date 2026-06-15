using RepositoryTier.User.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.User.Results
{
    public class RegisterAdminResult
    {
        public enRegisterAdminStatus Status { get; set; }
        public int? Id { get; set; }
        public RegisterAdminResult(enRegisterAdminStatus status,int? Id=null)
        {
            this.Id=Id;
            this.Status = status;
        }

    }
}
