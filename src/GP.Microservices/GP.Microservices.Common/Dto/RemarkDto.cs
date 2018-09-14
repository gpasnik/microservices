using System;
using System.Collections.Generic;

namespace GP.Microservices.Common.Dto
{
    public class RemarkDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string Status { get; set; }

        public Guid Author { get; set; }

        public Guid CategoryId { get; set; }

        public string CategoryName { get; set; }

        public IEnumerable<ActivityDto> Activities { get; set; }

        public IEnumerable<CommentDto> Comments { get; set; }

        public IEnumerable<ImageDto> Images { get; set; }
    }
}