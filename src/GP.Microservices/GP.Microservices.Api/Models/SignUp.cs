using System.ComponentModel.DataAnnotations;

namespace GP.Microservices.Api.Models
{
    public class SignUp
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }
    }
}