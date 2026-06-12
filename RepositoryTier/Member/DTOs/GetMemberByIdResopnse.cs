using RepositoryTier.Coach.DTOs;
using RepositoryTier.CustomAttributes;
using RepositoryTier.Member.Enums;
using RepositoryTier.User.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.Member.DTOs
{
    public class GetMemberByIdResopnse
    { 
        public int Id { get; set; }
        public string Email { get; set; } 
        public string FullName { get; set; } 
        public string Phone { get; set; } 
        public enGender Gender { get; set; } 
        public DateOnly DateOfBirth { get; set; } 
        public string Address { get; set; } 
        public decimal Height { get; set; } 
        public enMemberFitnessGoal FitnessGoal { get; set; } 
        public int? CoachId { get; set; }
        public bool IsActive { get; set; }

        public List<CoachLookUpResponse> Coaches { get; set; }
    }
}
