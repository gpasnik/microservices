using System;

namespace GP.Microservices.Common.Messages.Remarks.Events
{
    public class RemarkCanceled
    {
        public Guid RemarkId { get; set; }
    }
}