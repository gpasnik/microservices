using System;

namespace GP.Microservices.Common.Messages.Remarks.Events
{
    public class CommentRemoved
    {
        public Guid RemarkId { get; set; }

        public Guid CommentId { get; set; }
    }
}