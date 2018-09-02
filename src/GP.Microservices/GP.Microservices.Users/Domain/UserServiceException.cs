using System;

namespace GP.Microservices.Users.Domain
{
    public class UserServiceException : Exception
    {
        public Guid? UserId { get; set; }

        public string Username { get; set; }
    }
}