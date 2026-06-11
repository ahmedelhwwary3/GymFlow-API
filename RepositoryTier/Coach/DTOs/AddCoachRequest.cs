 
using RepositoryTier.Coach.Enums;
using RepositoryTier.CustomAttributes;
using RepositoryTier.User.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.Coach.DTOs
{
    public class AddCoachRequest
    {
        [EnumDataType(typeof(enCoachSpecialization))]
        public enCoachSpecialization Specialization { get; set; }

        [Range(1,int.MaxValue)] 
        public decimal Salary { get; set; }

        [Required]
        public string FullName { get; set; } = null!;

        [EmailAddress]
        [Required]
        public string Email { get; set; } = null!;

        [Phone]
        public string Phone { get; set; } = null!;

        [EnumDataType(typeof(enGender))]
        public enGender Gender { get; set; }

        [AgeRange(21,60)]
        public DateOnly DateOfBirth { get; set; }

        [Required]
        [MinLength(8)]
        public string Password { get; set; } = null!;    
    }
}
