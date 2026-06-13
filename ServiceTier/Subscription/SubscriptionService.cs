
using RepositoryTier.Coach.Repositories;
using RepositoryTier.Member.Repositories;
using RepositoryTier.Subscription.DTOs;
using RepositoryTier.Subscription.Repositories;
using RepositoryTier.Subscription.Results;
using System;
using System.Collections.Generic;
using RepositoryTier.Subscription.Enums;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RepositoryTier.Entities;

namespace ServiceTier.Subscription
{
    public class SubscriptionService : Service<RepositoryTier.Entities.Subscription>, ISubscriptionService
    {
        private readonly ISubscriptionRepository _repo;
        private readonly IMemberRepository _memberRepo;
        private readonly ICoachRepository _coachRepo;
        public SubscriptionService(
            ISubscriptionRepository repo,
            IMemberRepository memberRepo,
            ICoachRepository coachRepo
            ) 
            : base(repo) 
        { 
            _repo=repo;
            _coachRepo = coachRepo;
            _memberRepo=memberRepo;
        }

        public async Task<GetSubscriptionsResponse> 
            GetSubscriptionsAsync(GetSubscriptionsRequest request)
        {
            return await _repo.GetSubscriptionsAsync(request);
        }

        public DateOnly GetEndDate(enSubscriptionPlan plan,DateOnly start)
        {
            return plan switch
            {
                enSubscriptionPlan.Monthly => start.AddMonths(1),
                enSubscriptionPlan.Quarterly => start.AddMonths(3),
                enSubscriptionPlan.SemiAnnual => start.AddMonths(6),
                _ => start.AddYears(1)
            };
        }
        public async Task<AddSubscriptionResult> AddAsync(AddSubscriptionRequest request)
        {
            //1.Check existed active member
            var member = await _memberRepo.FindAsync(request.MemberId);
            if (member == null)
                return new AddSubscriptionResult(enAddSubscriptionStatus.MemberNotFound); 
            if (!member.IsActive)
                return new AddSubscriptionResult(enAddSubscriptionStatus.MemberInactive);

            //2.Check existed active attached coach
            if (member.CoachId == null)
                return new AddSubscriptionResult(enAddSubscriptionStatus.MemberNotAttachedToCoach);
            var coach = await _coachRepo.FindAsync(member.CoachId.Value);
            if(coach == null )
                return new AddSubscriptionResult(enAddSubscriptionStatus.CoachNotFound); 
            if(!coach.IsActive)
                return new AddSubscriptionResult(enAddSubscriptionStatus.CoachInctive);

            //3.Old subscriptions must be expired
            bool hasActiveOrForzenSubscriptions = await _memberRepo
                .HasActiveOrForzenSubscriptionsAsync(request.MemberId);
            if(hasActiveOrForzenSubscriptions)
                return new AddSubscriptionResult(enAddSubscriptionStatus.HasActiveOrForzenSubscription);

            //4.Create subscription
            var newSubscription = new RepositoryTier.Entities.Subscription()
            {
                CreatedAt = DateTime.UtcNow,
                CoachId = member.CoachId.Value,
                EndDate = GetEndDate(request.SubscriptionPlan,request.StartDate),
                MemberId=request.MemberId,
                Price=request.Price,
                SubscriptionPlan=request.SubscriptionPlan,
                StartDate=request.StartDate
            }; 
            await _repo.AddAsync(newSubscription); 
            int affectedRows = await _repo.SaveChangesAsync();

            return new AddSubscriptionResult(enAddSubscriptionStatus.Succeeded,newSubscription.Id);
        }

        public async Task<enFreezeSubscriptionStatus> 
            FreezeSubscriptionAsync(int Id, FreezeSubscriptionByIdRequest request)
        {
            //1.Chech exists
            var subscription = await _repo.FindAsync(Id);
            if (subscription == null)
                return enFreezeSubscriptionStatus.SubscriptionNotFound;

            //2.Chech status (active and not frozen)
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            bool isFrozen = subscription.FreezeEndDate.HasValue && subscription.FreezeEndDate > today;
            bool isExpired = subscription.FreezeEndDate == null && today >= subscription.EndDate;
            if (isFrozen)
                return enFreezeSubscriptionStatus.SubscriptionAlreadyFrozen;
            if (isExpired)
                return enFreezeSubscriptionStatus.SubscriptionExpired;

            //3.Load Then Update Strategy & save
            subscription.FreezeStartDate = request.FreezeStartDate;
            subscription.FreezeEndDate = request.FreezeEndDate;
            int affectedRows = await _repo.SaveChangesAsync();
            return enFreezeSubscriptionStatus.Succeeded;
        }
    }
}
