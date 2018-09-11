namespace GP.Microservices.Common.Messages.Users.Commands
{
    public class AuthorizeUser
    {
        public string Username { get; set; }

        public string Password { get; set; }
    }
}