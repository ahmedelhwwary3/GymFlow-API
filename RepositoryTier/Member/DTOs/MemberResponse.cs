using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.Member.DTOs
{
    public class MemberResponse
    {
        public int Id { get; set; }

        public int? CoachId { get; set; }

        public string CoachName { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string Phone {  get; set; }
         
        public bool IsActive { get; set; }

    }
}
