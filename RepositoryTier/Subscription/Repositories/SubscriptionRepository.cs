using RepositoryTier.Subscription.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using RepositoryTier.Subscription.Enums;
using System.Text;
using Microsoft.Extensions.Options;
using RepositoryTier.API_Configurations;
using Microsoft.EntityFrameworkCore;
using RepositoryTier.Coach.DTOs;

namespace RepositoryTier.Subscription.Repositories
{
    public class SubscriptionRepository : Repository<Entities.Subscription>, ISubscriptionRepository
    {
        protected readonly PaganationOptions _paganationOptions;
        public SubscriptionRepository(
            GymManagementDbContext context,
            IOptions<PaganationOptions> paganationOptions
            ) 
            : base(context) 
        {
            _paganationOptions = paganationOptions.Value;

            if (_paganationOptions == null)
                throw new Exception("Paganation options is not configured");
        }

        public async Task<GetSubscriptionsResponse> 
            GetSubscriptionsAsync(GetSubscriptionsRequest request)
        {
            int page = request.Page ?? _paganationOptions.Page;
            int pageSize = request.PageSize ?? _paganationOptions.BigPageSize;

            var query = _context.Subscriptions
                .Where(s =>
                (request.MemberId == null || s.MemberId == request.MemberId) &&
                (string.IsNullOrEmpty(request.Search) || s.Member.FullName.Contains(request.Search.Trim())));

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

            var shapedQuery = query.Select(s => new SubscriptionResponse()
            {
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

            int totalCount = await _context.Subscriptions.CountAsync();
            var coaches = await _context.Coaches.Select(c => new CoachLookUpResponse()
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
    }
}
