using System;

namespace GP.Microservices.Common.Messages.Remarks.Events
{
    public class ImageRemoved
    {
        public Guid RemarkId { get; set; }

        public Guid ImageId { get; set; }
    }
}