using System;

namespace GP.Microservices.Common.Messages.Remarks.Commands
{
    public class AddActivity
    {
        public DateTime Date { get; set; }

        public Guid ActivityTypeId { get; set; }

        public Guid RemarkId { get; set; }

        public Guid UserId { get; set; }
    }
}