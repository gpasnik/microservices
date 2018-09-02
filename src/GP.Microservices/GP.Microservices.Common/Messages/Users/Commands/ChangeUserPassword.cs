namespace GP.Microservices.Common.Messages.Users.Commands
{
    public class ChangeUserPassword
    {
        public string Username { get; set; }

        public string NewPassword { get; set; }
    }
}