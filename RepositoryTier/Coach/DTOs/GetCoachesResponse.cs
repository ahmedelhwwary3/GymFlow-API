using RepositoryTier.Coach.Enums;
using RepositoryTier.User.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.Coach.DTOs
{
    public class GetCoachesResponse
    {
        public List<CoachResponse> Coaches { get; set; } 
        public List<enCoachSpecialization> Specializations { get; set; }
    }
}
