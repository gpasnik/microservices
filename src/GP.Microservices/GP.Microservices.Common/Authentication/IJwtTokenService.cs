namespace GP.Microservices.Common.Authentication
{
    public interface IJwtTokenService
    {
        JsonWebToken Create(string requestUsername);
    }
}