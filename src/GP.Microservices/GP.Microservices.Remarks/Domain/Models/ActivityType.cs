using System;
using System.ComponentModel.DataAnnotations;

namespace GP.Microservices.Remarks.Domain.Models
{
    public class ActivityType
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }
    }
}