using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.DTOs.Users
{
    public class ChangePasswordRequest
    {
        [Range(1,int.MaxValue,ErrorMessage ="Invalid user id")]
        public int UserId { get; set; }

        [Required]
        public string CurrentPassword { get; set; }

        [Required]
        [StringLength(8)]
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; } 
    }
}
