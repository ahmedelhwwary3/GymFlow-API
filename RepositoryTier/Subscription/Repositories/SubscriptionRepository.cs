using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RepositoryTier.API_Configurations;
using RepositoryTier.Coach.DTOs;
using RepositoryTier.Subscription.DTOs;
using RepositoryTier.Subscription.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryTier.Subscription.Repositories
{
    public class SubscriptionRepository : Repository<Entities.Subscription>, ISubscriptionRepository
    {
        protected readonly PaganationOptions PaganationOptions;
        private readonly ILogger _logger;
        public SubscriptionRepository(
            GymManagementDbContext context,
            IOptions<PaganationOptions> paganationOptions,
            ILogger logger) 
            : base(context) 
        {
            _logger = logger;
            PaganationOptions = paganationOptions.Value;
            if (PaganationOptions == null)
            {
                _logger.LogError("Paganation options is not configured");
                throw new Exception("Paganation options is not configured");
            }
        }

        public async Task<GetSubscriptionsResponse> 
            GetSubscriptionsAsync(GetSubscriptionsRequest request)
        {
            int page = request.Page ?? PaganationOptions.Page;
            int pageSize = request.PageSize ?? PaganationOptions.BigPageSize;

            var query = _context.Subscriptions
                .IgnoreQueryFilters()
                .AsNoTracking()
                .Where(s =>
                (request.MemberId == null || s.MemberId == request.MemberId) &&
                (string.IsNullOrEmpty(request.Search) || s.Member.FullName.Contains(request.Search.Trim())) &&
                (request.Plan == null || s.SubscriptionPlan == request.Plan));

            DateOnly today = DateOnly.FromDateTime(DateTime.UtcNow);
            if (request.Status != null) // null means All
            {
                query = request.Status switch
                {
                    enSubscriptonStatus.Active => query.Where(s =>
                    s.EndDate > today && (s.FreezeEndDate == null || s.FreezeEndDate <= today)), // To avoid Frozen Status
                    enSubscriptonStatus.Expired => query.Where(s =>
                    s.EndDate <= today),
                    _ => query.Where(s =>
                    s.FreezeEndDate.HasValue && s.FreezeEndDate > today)
                };
            }

            query = request.Sort switch
            {
                enSubscriptonSort.EndDate => query.OrderBy(s => s.EndDate),
                enSubscriptonSort.StartDate => query.OrderBy(s => s.StartDate),
                enSubscriptonSort.MemberName => query.OrderBy(s => s.Member.FullName),
                _ => query
            };

            int totalCount = await _context.Subscriptions.CountAsync();

            var shapedQuery = query
                .IgnoreQueryFilters()
                .AsNoTracking()
                .Select(s => new SubscriptionResponse()
            {
                Plan=s.SubscriptionPlan,
                CoachName = s.Coach.FullName,
                EndDate = s.EndDate,
                Id = s.Id,
                MemberName = s.Member.FullName,
                Price = s.Price,
                StartDate = s.StartDate,
                Status = (s.EndDate <= today && (s.FreezeEndDate == null || s.FreezeEndDate < today)) ? enSubscriptonStatus.Active :
                s.FreezeEndDate.HasValue && s.FreezeEndDate > today ? enSubscriptonStatus.Frozen : enSubscriptonStatus.Expired
            });

            var subscriptions = await shapedQuery
                 .Skip((page - 1) * pageSize)
                 .Take(pageSize).ToListAsync();

            var coaches = request.MemberId.HasValue ? null :
                await _context.Coaches.Select(c => new CoachLookUpResponse()
                {
                    FullName = c.FullName,
                    Id = c.Id
                }).ToListAsync();

            return new GetSubscriptionsResponse()
            {
                Count = totalCount,
                Subscriptions = subscriptions,
                Coaches= coaches
            };
        }

        public async Task<GetSubscriptionByIdResponse?> GetByIdAsync(int Id)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            return await _context.Subscriptions
                .IgnoreQueryFilters()
                .AsNoTracking()
                .Where(s => s.Id == Id)
                .Select(s => new GetSubscriptionByIdResponse()
                {
                    CoachId = s.CoachId,
                    CoachName = s.Coach.FullName,
                    EndDate = s.EndDate,
                    Id = s.Id,
                    StartDate = s.StartDate,
                    MemberId = s.MemberId,
                    MemberName = s.Member.FullName,
                    MemberPhone = s.Member.Phone,
                    SubscriptionPrice = s.Price,
                    Plan = s.SubscriptionPlan,
                    TotalPaid = s.Payments.Sum(p => p.Amount),
                    Status = (s.EndDate <= today && (s.FreezeEndDate == null || s.FreezeEndDate < today)) ? enSubscriptonStatus.Active :
                s.FreezeEndDate.HasValue && s.FreezeEndDate > today ? enSubscriptonStatus.Frozen : enSubscriptonStatus.Expired
                }).FirstOrDefaultAsync();
        }
    }
}
