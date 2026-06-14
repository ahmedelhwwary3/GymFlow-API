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
    public class AddCoachResponse
    {
        public int Id { get; set; }
         
        public enCoachSpecialization Specialization { get; set; }

        public DateOnly HireDate { get; set; }

        public decimal Salary { get; set; }
    }
}
