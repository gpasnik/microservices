﻿using System;

namespace GP.Microservices.Common.Messages.Remarks.Events
{
    public class ResolveRemarkFailed
    {
        public Guid RemarkId { get; set; }

        public Guid UserId { get; set; }
    }
}