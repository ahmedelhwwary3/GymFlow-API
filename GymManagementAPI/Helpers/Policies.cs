namespace GymManagementAPI.Helpers
{
    public static class Policies
    {
        public const string OwnerOrAdmin = nameof(OwnerOrAdmin); 

        public const string SubscriptionOwnerOrAdmin = nameof(SubscriptionOwnerOrAdmin);

        public const string WorkoutPlanOwnerOrAdmin = nameof(WorkoutPlanOwnerOrAdmin);

        public const string Cors = nameof(Cors);
    }
}
