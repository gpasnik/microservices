using System;

namespace GP.Microservices.Common.Dto
{
    public class UserDto
    {
        public Guid Id { get; set; }

        public string Username { get; set; }

        public string Name { get; set; }

        public string Lastname { get; set; }

        public string Email { get; set; }

        public string Status { get; set; }
    }
}