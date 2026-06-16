using RepositoryTier.User.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.User.DTOs
{
    public class UpdateUserRequest
    {
        public string FullName { get; set; } 

        public string Email { get; set; }  

        public string Phone { get; set; }  

        public enGender Gender { get; set; }

        public DateOnly DateOfBirth { get; set; } 

        public enUserRole Role { get; set; }

        public bool IsActive { get; set; }  
    }
}
