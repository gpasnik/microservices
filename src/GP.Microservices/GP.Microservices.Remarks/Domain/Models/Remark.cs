﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Spatial;

namespace GP.Microservices.Remarks.Domain.Models
{
    public class Remark
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public RemarkStatus Status { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateUpdated { get; set; }

        public Guid CategoryId { get; set; }

        public Guid AuthorId { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        public Remark()
        {
            
        }

        public Remark(string name, string description, double latitude, double longitude, Guid categoryId, Guid authorId)
        {
            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            Latitude = latitude;
            Longitude = longitude;
            CategoryId = categoryId;
            AuthorId = authorId;
            DateCreated = DateTime.UtcNow;
            Status = RemarkStatus.Reported;
        }
    }
}