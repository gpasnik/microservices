using System;

namespace GP.Microservices.Common.Dto
{
    public class CommentDto
    {
        public Guid Id { get; set; }

        public string Text { get; set; }

        public string Status { get; set; }

        public Guid AuthorId { get; set; }

        public string AuthorName { get; set; }
    }
}