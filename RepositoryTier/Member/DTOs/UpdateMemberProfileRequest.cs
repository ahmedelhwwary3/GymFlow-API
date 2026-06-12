using RepositoryTier.Member.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RepositoryTier.Member.DTOs
{
    public class UpdateMemberProfileRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Phone]
        public string Phone { get; set; }

        [Required]
        [Length(5,100)]
        public string Address { get; set; }

        [EnumDataType(typeof(enMemberFitnessGoal))]
        public enMemberFitnessGoal FitnessGoal { get; set; }

        [Range(1,int.MaxValue)]
        public decimal Height { get; set; }
    }
}
