using System;

namespace GP.Microservices.Common.Messages.Remarks.Events
{
    public class ImageAdded
    {
        public Guid RemarkId { get; set; }

        public Guid ImageId { get; set; }

        public string Url { get; set; }
    }
}