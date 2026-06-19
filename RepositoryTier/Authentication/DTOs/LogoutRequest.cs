using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.Authentication.DTOs
{
    public class LogoutRequest
    {
        [Required]
        public string RefreshToken { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }
    }
}
