using RepositoryTier.Coach.DTOs;
using RepositoryTier.User.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.Member.DTOs
{
    public class GetMembersResopnse
    {
        public List<MemberResponse> Members {  get; set; } 
        public List<CoachLookUpResponse> Coaches { get; set; }
        public int Count { get; set; }
    }
}
