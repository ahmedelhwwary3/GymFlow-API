using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepositoryTier.Member.Enums;

namespace RepositoryTier.Member.Results
{
    public class AddMemberResult
    {
        public enAddMemberStatus Status { get; set; }
        public Nullable<int> Id { get; set; }
        public AddMemberResult(enAddMemberStatus status,int? newId=null)
        {
            this.Status = status;
            this.Id = newId;
        }
    }
}
