using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.Member.DTOs
{
    public class GetAssignedMembersForCoachRequest
    {
        public int? Page {  get; set; }

        public int? PageSize { get; set; }

        public int CoachId { get; set; }
    }
}
