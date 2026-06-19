namespace GymManagementAPI.Extensions
{
    public static class IloggerExtensions
    {
        public static void LogAdminInvalidAction(
            this ILogger logger, string action, 
            enInvalidAdminActionReason reason, string adminId, string IP)
        {
            logger.LogWarning($"Admin action blocked ({GetAdminActionBlockReason(reason)})." +
                $" AdminId={adminId}, Action={action}, IP={IP}");
        }

        public static void LogAdminExecutedAction(
            this ILogger logger, string action, string adminId, string IP)
        {
            logger.LogInformation($"Admin action executed." +
                $" AdminId={adminId}, Action={action}, IP={IP}");
        }

        public static string GetAdminActionBlockReason
            (enInvalidAdminActionReason reason)
        {
            return reason switch
            {
                enInvalidAdminActionReason.InvalidInput => "Invalid Input",
                enInvalidAdminActionReason.LogicalError => "Logical Error",
                _ => "N\\A"
            };
        }

        public enum enInvalidAdminActionReason
        {
            InvalidInput = 1,
            LogicalError = 2
        }
    }
}
