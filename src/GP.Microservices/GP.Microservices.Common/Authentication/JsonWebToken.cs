namespace GP.Microservices.Common.Authentication
{
    public class JsonWebToken
    {
        public string Token { get; set; }

        public long Expires { get; set; }
    }
}