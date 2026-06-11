using RepositoryTier.Coach.Enums;
using RepositoryTier.User.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RepositoryTier.Coach.DTOs
{
    public class GetCoachByIdResponse
    {
        public int Id { get; set; }
        public string FullName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public enUserGender Gender { get; set; }

        public DateOnly HireDate { get; set; }

        public enCoachSpecialization Specialization { get; set; }

        public decimal Salary { get; set; }

        public enUserActivityStatus Status { get; set; }


    }
}
