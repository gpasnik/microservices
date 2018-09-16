using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GP.Microservices.Remarks.Domain.Models
{
    public class Image
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public DateTime DateCreated { get; set; }

        public Guid RemarkId { get; set; }

        public Guid? ActivityId { get; set; }

        public Guid AuthorId { get; set; }

        [ForeignKey("RemarkId")]
        public virtual Remark Remark { get; set; }

        public Image()
        {
            
        }

        public Image(string name, string url, Guid remarkId, Guid authorId, Guid? activityId = null)
        {
            Id = Guid.NewGuid();
            Name = string.IsNullOrWhiteSpace(name)
                ? Id.ToString()
                : name;
            Url = url;
            DateCreated = DateTime.UtcNow;
            RemarkId = remarkId;
            ActivityId = activityId;
            AuthorId = authorId;
        }
    }
}