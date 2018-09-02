using System;

namespace GP.Microservices.Common.Messages.Remarks.Commands
{
    public class AddImage
    {
        public Guid RemarkId { get; set; }

        public Guid UserId { get; set; }

        public Guid? ActivityId { get; set; }

        public string Base64 { get; set; }

        public string Name { get; set; }

        public int? Order { get; set; }
    }
}