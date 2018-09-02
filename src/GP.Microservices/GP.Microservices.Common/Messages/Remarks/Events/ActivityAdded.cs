using System;

namespace GP.Microservices.Common.Messages.Remarks.Events
{
    public class ActivityAdded
    {
        public Guid RemarkId { get; set; }

        public Guid ActivityId { get; set; }
    }
}