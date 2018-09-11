using System;
using GP.Microservices.Common.ServiceClients;

namespace GP.Microservices.Common.Exceptions
{
    public class ServiceException : Exception
    {
        public ServiceException(string message, ServiceError error, Exception innerException)
            :base(message, innerException)
        {
            ServiceError = error;
        }

        public ServiceException(string message, ServiceError error)
            :base(message)
        {
            ServiceError = error;
        }

        public ServiceException(ServiceError error)
            :base(error.Message)
        {
            ServiceError = error;
        }

        public ServiceError ServiceError { get; }
    }
}