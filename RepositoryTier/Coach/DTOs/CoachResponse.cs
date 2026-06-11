using RepositoryTier.Coach.Enums;
using RepositoryTier.User.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.Coach.DTOs
{
    public class CoachResponse
    {
        public int Id { get; set; }
        public string FullName {  get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public enCoachSpecialization Specialization { get; set; }
        public int AssignedMembersCount { get; set; } = 0;
        public enUserActivityStatus Status { get; set; }
    }
}
