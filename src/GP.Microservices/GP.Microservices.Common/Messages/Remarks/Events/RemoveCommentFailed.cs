using System;

namespace GP.Microservices.Common.Messages.Remarks.Events
{
    public class RemoveCommentFailed
    {
        public Guid RemarkId { get; set; }

        public Guid UserId { get; set; }

        public Guid CommentId { get; set; }
    }
}