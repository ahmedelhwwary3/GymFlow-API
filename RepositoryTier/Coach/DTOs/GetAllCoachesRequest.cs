using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepositoryTier.Coach.Enums;

namespace RepositoryTier.Coach.DTOs
{
    public class GetAllCoachesRequest
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string? Search { get; set; }
        public int Status { get; set; }
        public enCoachSpecialization Specialization { get; set; }

    }
}
