using RepositoryTier.Member.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.Member.DTOs
{
    public class GetMemberProfileResopnse
    {  
        public int Id { get; set; } // We may change Cookie in the future (no dependency on JWT)
        public string Email { get; set; } 
        public string Phone { get; set; } 
        public string Address { get; set; } 
        public enMemberFitnessGoal FitnessGoal { get; set; } 
        public decimal Height { get; set; } 
        public string CoachName { get; set; }
    }
}
