using System;

namespace GP.Microservices.Common.Messages.Remarks.Events
{
    public class RemoveImageFailed
    {
        public Guid RemarkId { get; set; }

        public Guid UserId { get; set; }

        public Guid ImageId { get; set; }
    }
}