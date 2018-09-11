using System;

namespace GP.Microservices.Common.Authentication
{
    public interface IJwtTokenService
    {
        JsonWebToken Create(Guid userId, string username);
    }
}