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

        public CommentStatus Status { get; set; }

        public DateTime DateCreated { get; set; }

        public Guid RemarkId { get; set; }

        public Guid AuthorId { get; set; }

        [ForeignKey("RemarkId")]
        public virtual Remark Remark { get; set; }

        public Comment()
        {
            
        }

        public Comment(string text, Guid remarkId, Guid authorId)
        {
            Id = Guid.NewGuid();
            Text = text;
            Status = CommentStatus.Active;
            DateCreated = DateTime.UtcNow;
            RemarkId = remarkId;
            AuthorId = authorId;
        }
    }
}