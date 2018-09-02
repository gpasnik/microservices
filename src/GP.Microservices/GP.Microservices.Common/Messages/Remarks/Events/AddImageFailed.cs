using System;

namespace GP.Microservices.Common.Messages.Remarks.Events
{
    public class AddImageFailed
    {
        public Guid RemarkId { get; set; }

        public Guid UserId { get; set; }

        public Guid? ImageId { get; set; }
    }
}