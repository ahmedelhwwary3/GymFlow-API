using System;
using System.Collections.Generic;

namespace RepositoryTier.Models;

public partial class Attendance
{
    public int Id { get; set; }

    public int MemberId { get; set; }

    public DateOnly AttendanceDate { get; set; }

    public string? Notes { get; set; }

    public virtual Member Member { get; set; } = null!;

    
}
