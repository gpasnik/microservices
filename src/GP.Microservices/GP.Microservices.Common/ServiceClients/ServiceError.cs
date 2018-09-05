using System;

namespace GP.Microservices.Common.ServiceClients
{
    public class ServiceError
    {
        public string Service { get; set; }

        public string Type { get; set; }

        public string Code { get; set; }

        public string Message { get; set; }

        public Guid TraceId { get; set; }
    }
}