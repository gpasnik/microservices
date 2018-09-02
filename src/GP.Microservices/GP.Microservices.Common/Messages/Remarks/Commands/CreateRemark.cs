using System;

namespace GP.Microservices.Common.Messages.Remarks.Commands
{
    public class CreateRemark
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public Guid CategoryId { get; set; }
    }
}