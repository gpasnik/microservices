using System;
using System.Collections.Generic;

namespace GP.Microservices.Common.Messages.Remarks.Queries
{
    public class BrowseRemarks
    {
        public double? Radius { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public IEnumerable<Guid> Categories { get; set; }
        public IEnumerable<Guid> Statuses { get; set; }
        public IEnumerable<string> Authors { get; set; }
    }
}