using System;
using System.ComponentModel.DataAnnotations;

namespace GP.Microservices.Api.Models
{
    public class AddImageRequest
    {
        [Required]
        public string Base64 { get; set; }

        public string Name { get; set; }

        public int? Order { get; set; }

        public Guid? ActivityId { get; set; }
    }
}