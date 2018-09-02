using System;

namespace GP.Microservices.Common.Messages.Users.Events
{
    public class UserDeleted
    {
        public Guid Id { get; set; }

        public string Username { get; set; }
    }
}