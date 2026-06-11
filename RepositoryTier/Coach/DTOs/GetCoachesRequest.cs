using RepositoryTier.Coach.Enums;
using RepositoryTier.User.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.Coach.DTOs
{
    public class GetCoachesRequest
    { 
        public string? Search { get; set; } 

        public bool? IsActive { get; set; }

        public enCoachSpecialization? Specialization { get; set; }

        public int? Page { get; set; } 

        public int? PageSize { get; set; }

        [EnumDataType(typeof(enCoachSort))]
        public enCoachSort? Sort { get; set; }


    }
}
