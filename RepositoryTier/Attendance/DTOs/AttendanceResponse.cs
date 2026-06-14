using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.Attendance.DTOs
{
    public class AttendanceResponse
    {  
        public int AttendanceId { get; set; }
        public string MemberName { get; set; }  
        public int CoachId { get; set; }
        public string CoachName { get; set; }
        public DateOnly AttendanceDate { get; set; }

    }
}
