using System;
using System.ComponentModel.DataAnnotations;

namespace GP.Microservices.Api.Models
{
    public class AddActivityRequest
    {
        [Required]
        public Guid ActivityTypeId { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}