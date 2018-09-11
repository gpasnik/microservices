using System.ComponentModel.DataAnnotations;

namespace GP.Microservices.Api.Models
{
    public class SignIn
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}