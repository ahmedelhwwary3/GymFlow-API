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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RepositoryTier.Coach.DTOs
{
    public class UpdateCoachByIdRequest
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        [EmailAddress] 
        public string Email { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        [NotFutureDateOnlyAttribute]
        public DateOnly HireDate { get; set; }

        [EnumDataType(typeof(enCoachSpecialization))]
        public enCoachSpecialization Specialization { get; set; }

        [Range(1,int.MaxValue)]
        public decimal Salary { get; set; } 

        public bool IsActive { get; set; }
    }
}
