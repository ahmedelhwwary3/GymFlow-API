using RepositoryTier.Member.Enums;
using RepositoryTier.User.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using RepositoryTier.CustomAttributes;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RepositoryTier.Member.DTOs
{
    public class AddMemberRequest
    { 
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(8)]
        public string Password { get; set; }

        [Required] 
        [Length(2,100)]
        public string FullName { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        public enGender Gender { get; set; }

        [Required]
        [NotFutureDateOnlyAttribute] 
        public DateOnly DateOfBirth { get; set; }

        [Required]
        [MinLength(5)]
        public string Address { get; set; }

        [Range(1,int.MaxValue)]
        public decimal Height { get; set; }

        [EnumDataType(typeof(enMemberFitnessGoal))]
        public enMemberFitnessGoal FitnessGoal { get; set; }

        [Range(1,int.MaxValue)]
        public int CoachId { get; set; }
         
    }
}
