using System;

namespace GP.Microservices.Common.Dto
{
    public class ImageDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public int Order { get; set; }
    }
}