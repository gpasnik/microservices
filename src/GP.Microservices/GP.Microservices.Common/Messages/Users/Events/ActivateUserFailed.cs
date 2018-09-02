using System;

namespace GP.Microservices.Common.Messages.Users.Events
{
    public class ActivateUserFailed
    {
        public Guid? Id { get; set; }

        public string Username { get; set; }

        public string ActivationToken { get; set; }
    }
}