using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepositoryTier.Coach.Enums;
using RepositoryTier.User.Enums;

namespace RepositoryTier.Coach.DTOs
{
    public class GetCoachesRequest
    { 
        public string? Search { get; set; }
        public enUserActivityStatus Status { get; set; } = enUserActivityStatus.All;
        public enCoachSpecialization? Specialization { get; set; }
        public int? Page { get; set; } 
        public int? PageSize { get; set; }
        public enCoachSort? Sort { get; set; }


    }
}
