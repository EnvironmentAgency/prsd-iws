﻿namespace EA.Iws.Requests.Movement.Reject
{
    using System;
    using Prsd.Core;
    using Prsd.Core.Mediator;

    public class RecordRejectionInternal : IRequest<bool>
    {
        public Guid MovementId { get; private set; }

        public string RejectionFurtherInformation { get; private set; }

        public string RejectionReason { get; private set; }

        public DateTime RejectedDate { get; private set; }

        public RecordRejectionInternal(Guid movementId, 
            DateTime rejectedDate, 
            string rejectionReason, 
            string rejectionFurtherInformation)
        {
            Guard.ArgumentNotNullOrEmpty(() => rejectionReason, rejectionReason);

            MovementId = movementId;
            RejectedDate = rejectedDate;
            RejectionReason = rejectionReason;
            RejectionFurtherInformation = rejectionFurtherInformation;
        }
    }
}