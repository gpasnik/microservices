using System;

namespace GP.Microservices.Common.Messages.Remarks.Commands
{
    public class RemoveComment
    {
        public Guid CommentId { get; set; }

        public Guid UserId { get; set; }
    }
}