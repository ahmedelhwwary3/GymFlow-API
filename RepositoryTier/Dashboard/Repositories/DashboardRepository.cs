using Microsoft.EntityFrameworkCore;
using RepositoryTier.Dashboard.DTOs;
using RepositoryTier.Subscription.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 

namespace RepositoryTier.Dashboard.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly GymManagementDbContext _context;
        public DashboardRepository(GymManagementDbContext context)
        {
            _context = context;
        }

        public async Task<AdminDashboardResponse> GetForAdminAsync()
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            int totalMembers = await _context.Members
                .CountAsync();
            int activeMembers = await _context.Members
                .CountAsync(m => m.IsActive);

            int totalCoaches = await _context.Coaches
                .CountAsync();
            int activeCoaches = await _context.Coaches
                .CountAsync();

            int activeSubscriptions = await _context.Subscriptions
                .CountAsync(s => s.FreezeEndDate == null && s.EndDate > today);
            int frozenSubscriptions = await _context.Subscriptions
              .CountAsync(s => s.FreezeEndDate != null && s.FreezeEndDate > today);
            int expiredSubscriptions = await _context.Subscriptions
              .CountAsync(s => s.FreezeEndDate == null && s.EndDate <= today);

            decimal totalRevenue = await _context.Payments
                .SumAsync(p => p.Amount);
            decimal paymentsThisMonth = await _context.Payments
                .Where(p => p.PaymentDate.Month == DateTime.UtcNow.Month &&
                p.PaymentDate.Year == DateTime.UtcNow.Year)
                .SumAsync(p => p.Amount);

            int attendanceToday = await _context.Attendances
                .Where(a => a.AttendanceDate.Day == DateTime.UtcNow.Day &&
                a.AttendanceDate.Year == DateTime.UtcNow.Year)
                .CountAsync();

            return new AdminDashboardResponse
            {
                ActiveCoaches=activeCoaches,
                ActiveMembers=activeMembers,
                ActiveSubscriptions=activeSubscriptions,
                AttendanceToday=attendanceToday,
                ExpiredSubscriptions=expiredSubscriptions,
                FrozenSubscriptions=frozenSubscriptions,
                PaymentsThisMonth=paymentsThisMonth,
                TotalCoaches=totalCoaches,
                TotalMembers=totalMembers,
                TotalRevenue=totalRevenue,
            }; 
        }

        public async Task<CoachDashboardResponse> GetForCoachAsync(int coachId)
        {
            int assignedMembers = await _context.Members
                .CountAsync(m => m.CoachId == coachId);
            int activeMembers = await _context.Members
                .CountAsync(m => m.CoachId == coachId && m.IsActive);

            int workoutPlansCreated = await _context.WorkoutPlans
                .CountAsync(m => m.CoachId == coachId);
            int activeWorkoutPlans = await _context.WorkoutPlans
                .CountAsync(m => m.CoachId == coachId && m.IsActive);

            return new CoachDashboardResponse
            {
                ActiveMembers= activeMembers,
                AssignedMembers=assignedMembers,
                ActiveWorkoutPlans= activeWorkoutPlans,
                WorkoutPlansCreated=workoutPlansCreated
            };
        }
        public async Task<MemberDashboardResponse?> GetForMemberAsync(int memberId)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            var query = _context.Members
                .IgnoreQueryFilters()
                .AsNoTracking()
                .Where(m => m.Id == memberId)
                .Select(m => new MemberDashboardResponse
                {
                    CoachId = m.CoachId,
                    CoachName = m.Coach.FullName,
                    SubscriptionPlan = m.Subscriptions
                    .OrderByDescending(s => s.Id)
                    .Select(s => s.SubscriptionPlan)
                    .FirstOrDefault(),

                    SubscriptonStatus= m.Subscriptions
                    .OrderByDescending(s => s.Id)
                    .Select(s => s.FreezeEndDate==null&&s.EndDate>today?enSubscriptonStatus.Active:
                    s.FreezeEndDate!=null&&s.FreezeEndDate>today?enSubscriptonStatus.Frozen
                    :enSubscriptonStatus.Expired)
                    .FirstOrDefault(),

                    SubscriptionEndDate = m.Subscriptions
                    .OrderByDescending(s => s.Id)
                    .Select(s => s.EndDate)
                    .FirstOrDefault(),

                    CurrentWorkoutPlanId = m.WorkoutPlans
                    .OrderByDescending(s => s.Id)
                    .Select(s => s.Id)
                    .FirstOrDefault(),

                    CurrentWorkoutPlanName = m.WorkoutPlans
                    .OrderByDescending(s => s.Id)
                    .Select(s => s.Name)
                    .FirstOrDefault(),

                    ExercisesCount = m.WorkoutPlans
                    .OrderByDescending(s => s.Id)
                    .FirstOrDefault()
                    .WorkoutPlanExercises
                    .Count(),

                    AttendanceThisMonth = m.Attendances
                    .Count(a => a.AttendanceDate.Month == DateTime.UtcNow.Month&&
                    a.AttendanceDate.Year== DateTime.UtcNow.Year),

                    LastAttendanceDate = m.Attendances
                    .OrderByDescending(a => a.Id)
                    .Select(a => a.AttendanceDate)
                    .FirstOrDefault()
                });
              
            var dto= await query.SingleOrDefaultAsync();

            int remaining = dto.SubscriptionEndDate.Value.Day - DateTime.UtcNow.Day;
            dto.RemainingDays= remaining<0?0 : remaining;
            return dto;
        }
    }
}
