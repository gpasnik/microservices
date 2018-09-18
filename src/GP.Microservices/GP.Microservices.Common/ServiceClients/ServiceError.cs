using System;
using System.Reflection;

namespace GP.Microservices.Common.ServiceClients
{
    public class ServiceError
    {
        public bool Success { get; private set; }

        public string Service { get; set; }

        public string Type { get; set; }

        public string Code { get; set; }

        public int HttpStatusCode { get; set; }

        public string Message { get; set; }

        public Guid TraceId { get; set; }


        public ServiceError()
        {
            Success = false;
            TraceId = Guid.NewGuid();
        }

        public ServiceError(string message, int httpStatusCode)
            :this()
        {
            Message = message;
            HttpStatusCode = httpStatusCode;
            Service = Assembly.GetEntryAssembly().FullName;
        }

        public ServiceError(string message, int httpStatusCode, string type, string code)
            :this(message, httpStatusCode)
        {
            Type = type;
            Code = code;
        }
    }
}