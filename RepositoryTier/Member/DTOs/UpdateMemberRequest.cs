using RepositoryTier.Member.Enums;
using RepositoryTier.User.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RepositoryTier.CustomAttributes;

namespace RepositoryTier.Member.DTOs
{
    public class UpdateMemberRequest
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }


        [EnumDataType(typeof(enGender))]
        public enGender Gender { get; set; }

        [NotFutureDateOnly]
        public DateOnly DateOfBirth { get; set; }

        [Length(5,100)]
        public string Address { get; set; }

        [Range(1, int.MaxValue)]
        public decimal Height { get; set; }

        [EnumDataType(typeof(enMemberFitnessGoal))]
        public enMemberFitnessGoal FitnessGoal { get; set; }

        public int? CoachId { get; set; }  

        public bool IsActive { get; set; } 
    }
}
