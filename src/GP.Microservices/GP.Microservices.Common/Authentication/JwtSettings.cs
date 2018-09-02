namespace GP.Microservices.Common.Authentication
{
    public class JwtSettings
    {
        public string Issuer { get; set; }

        public string SecretKey { get; set; }

        public int ExpiryHours { get; set; }
    }
}