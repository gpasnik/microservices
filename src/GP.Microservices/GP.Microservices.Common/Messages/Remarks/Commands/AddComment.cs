using System;

namespace GP.Microservices.Common.Messages.Remarks.Commands
{
    public class AddComment
    {
        public Guid RemarkId { get; set; }

        public Guid UserId { get; set; }

        public string Text { get; set; }
    }
}