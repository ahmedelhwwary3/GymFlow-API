using RepositoryTier.CustomAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.Attendance.DTOs
{
    public class AddAttendanceRequest
    { 
        public int? MemberId { get; set; } 

        public string? Search { get; set; } 

        public string? Notes { get; set; } = null;
    }
}
