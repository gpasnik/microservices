using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GP.Microservices.Remarks.Domain.Models
{
    public class Comment
    {
        [Key]
        public Guid Id { get; set; }

        public string Text { get; set; }

        public string Status { get; set; }

        public DateTime DateCreated { get; set; }

        public Guid RemarkId { get; set; }

        public Guid AuthorId { get; set; }

        [ForeignKey("RemarkId")]
        public virtual Remark Remark { get; set; }
    }
}