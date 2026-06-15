using RepositoryTier.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.Dashboard.DTOs
{
    public class AdminDashboardResponse
    {
        public int TotalMembers { get; set; }
        public int ActiveMembers { get; set; }
        public int InactiveMembers { get => TotalMembers - ActiveMembers; }

        public int TotalCoaches { get; set; }
        public int ActiveCoaches { get; set; }
        public int InactiveCoaches { get => TotalCoaches - ActiveCoaches; }

        public int ActiveSubscriptions { get; set; }
        public int ExpiredSubscriptions { get; set; }
        public int FrozenSubscriptions { get; set; }

        public decimal TotalRevenue { get; set; }
        public decimal PaymentsThisMonth { get; set; }

        public int AttendanceToday { get; set; }
    }
}
