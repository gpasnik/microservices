using System;

namespace GP.Microservices.Common.Models
{
    public class Operation
    {
        public Guid Id { get; set; }

        public Guid ResourceId { get; set; }

        public string Endpoint { get; set; }

        public OperationStatus Status { get; set; }

        public int Progress { get; set; }
    }

    public enum OperationStatus
    {
        Pending,
        Completed,
        Error
    }
}