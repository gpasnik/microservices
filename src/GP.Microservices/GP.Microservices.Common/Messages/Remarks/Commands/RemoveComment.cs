using System;

namespace GP.Microservices.Common.Messages.Remarks.Commands
{
    public class RemoveComment
    {
        public Guid RemarkId { get; set; }

        public Guid CommentId { get; set; }

        public Guid UserId { get; set; }
    }
}