﻿using System;

namespace GP.Microservices.Common.Messages.Remarks.Commands
{
    public class CancelRemark
    {
        public Guid RemarkId { get; set; }

        public Guid UserId { get; set; }
    }
}