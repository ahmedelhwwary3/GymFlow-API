using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepositoryTier.User.Enums;

namespace RepositoryTier.User.Results
{
    public class RegisterMemberResult
    {
        public enRegisterMemberStatus Status { get; set; }
        public Nullable<int> Id { get; set; }
        public RegisterMemberResult(enRegisterMemberStatus status,int? newId=null)
        {
            this.Status = status;
            this.Id = newId;
        }
    }
}
