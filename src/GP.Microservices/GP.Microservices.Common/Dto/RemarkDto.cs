using System;

namespace GP.Microservices.Common.Dto
{
    public class RemarkDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid Author { get; set; }
    }
}