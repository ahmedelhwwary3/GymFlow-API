using RepositoryTier.Subscription.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using RepositoryTier.Subscription.Enums;
using System.Text;
using Microsoft.Extensions.Options;
using RepositoryTier.API_Configurations;

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
            query = request.Status switch
            {
                enSubscriptonStatus.Active => query.Where(s =>
                s.EndDate > today && (s.FreezeEndDate == null || s.FreezeEndDate <= today)), // To avoid Frozen Status
                enSubscriptonStatus.Expired => query.Where(s =>
                s.EndDate <= today),
                enSubscriptonStatus.Frozen => query.Where(s =>
                s.FreezeEndDate.HasValue && s.FreezeEndDate > today),
                _ => query
            };
            //query = query.Select(s=>new SubscriptionResponse()
            //{
            //    //CoachName=s.Member.Coach.
            //});
            query = request.Sort switch
            {
                enSubscriptonSort.EndDate => query.OrderBy(s => s.EndDate),
                enSubscriptonSort.StartDate => query.OrderBy(s => s.StartDate),
                enSubscriptonSort.MemberName => query.OrderBy(s => s.Member.FullName),
                _ => query
            };

            query = query.Skip((page - 1) * pageSize).Take(pageSize);
            

        }
    }
}
