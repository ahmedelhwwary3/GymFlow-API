namespace RepositoryTier.API_Configurations
{
    public class JWTOptions
    {
        public string Issuer { get; set; } = ""; 
        public string Audience { get; set; } = "";
        public int ExpirationInMinutes { get; set; }
        public int RefreshTokenExpirationInDays { get; set; }
    }
}
