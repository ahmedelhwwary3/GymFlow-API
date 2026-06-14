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
        [Range(1,int.MaxValue)]
        public int MemberId { get; set; }

        public string? Notes { get; set; }
    }
}
