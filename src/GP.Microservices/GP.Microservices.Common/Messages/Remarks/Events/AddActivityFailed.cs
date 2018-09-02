using System;

namespace GP.Microservices.Common.Messages.Remarks.Events
{
    public class AddActivityFailed
    {
        public Guid RemarkId { get; set; }

        public Guid UserId { get; set; }
    }
}