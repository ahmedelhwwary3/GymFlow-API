using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.User.DTOs.Authentication
{
    public class LogoutRequest
    {
        [Required]
        public string RefreshToken { get; set; }

        public string Email { get; set; }
    }
}
