using System;

namespace GP.Microservices.Common.Messages.Users.Events
{
    public class UserBlocked
    {
        public Guid Id { get; set; }

        public string Username { get; set; }
    }
}