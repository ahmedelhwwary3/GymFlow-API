using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryTier.Subscription.Enums
{
    public enum enAddSubscriptionStatus
    {
        MemberNotFound = 1,
        MemberInactive = 2,
        MemberNotAttachedToCoach = 3,
        CoachNotFound = 4, 
        CoachInctive = 5, 
        HasActiveOrForzenSubscription = 6,
        Succeeded= 7
    }
}
