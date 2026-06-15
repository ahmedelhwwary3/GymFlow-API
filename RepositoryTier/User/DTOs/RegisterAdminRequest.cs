using RepositoryTier.User.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.User.DTOs
{
    public class RegisterAdminRequest
    {
        public string FullName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public enGender Gender { get; set; }

        public DateOnly DateOfBirth { get; set; }
         
        public string Password { get; set; } = null!;
    }
}
