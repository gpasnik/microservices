namespace GP.Microservices.Common.Messages.Users.Commands
{
    public class ActivateUser
    {
        public string Username { get; set; }

        public string ActivationToken { get; set; }
    }
}