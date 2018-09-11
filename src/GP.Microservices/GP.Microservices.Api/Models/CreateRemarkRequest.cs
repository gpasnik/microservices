using System;
using System.ComponentModel.DataAnnotations;

namespace GP.Microservices.Api.Models
{
    public class CreateRemarkRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public Guid CategoryId { get; set; }

        public string Description { get; set; }

        [Required]
        public double Longitude { get; set; }

        [Required]
        public double Latitude { get; set; }
    }
}