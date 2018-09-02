using System;

namespace GP.Microservices.Common.Messages.Remarks.Commands
{
    public class RemoveImage
    {
        public Guid ImageId { get; set; }

        public Guid UserId { get; set; }
    }
}