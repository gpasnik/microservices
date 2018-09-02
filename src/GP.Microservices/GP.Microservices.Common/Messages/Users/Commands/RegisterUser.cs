namespace GP.Microservices.Common.Messages.Users.Commands
{
    public class RegisterUser
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }
    }
}