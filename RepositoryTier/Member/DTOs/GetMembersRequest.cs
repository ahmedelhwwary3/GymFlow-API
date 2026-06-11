using RepositoryTier.Coach.Enums;
using RepositoryTier.Member.Enums;
using RepositoryTier.User.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.Member.DTOs
{
    public class GetMembersRequest
    {
        public string? Search {  get; set; }
        public bool? IsActive { get; set; }
        public int? CoachId { get; set; }
        public enGender Gender { get; set; }

        public enMemberSort Sort { get; set; }

        public int? Page { get; set; }
        public int? PageSize { get; set; }
    }
}
