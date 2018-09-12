using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GP.Microservices.Remarks.Domain.Models
{
    public class Activity
    {
        [Key]
        public Guid Id { get; set; }

        public DateTime Date { get; set; }

        public Guid RemarkId { get; set; }

        public Guid TypeId { get; set; }

        public Guid AuthorId { get; set; }

        [ForeignKey("RemarkId")]
        public virtual Remark Remark { get; set; }

        [ForeignKey("TypeId")]
        public virtual ActivityType Type { get; set; }
    }
}