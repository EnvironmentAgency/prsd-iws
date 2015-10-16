﻿namespace EA.Iws.Requests.MovementReceipt
{
    using System;
    using Prsd.Core.Mediator;

    public class SetMovementAccepted : IRequest<Guid>
    {
        public SetMovementAccepted(Guid movementId, Guid fileId, DateTime dateReceived, decimal quantity)
        {
            MovementId = movementId;
            FileId = fileId;
            DateReceied = dateReceived;
            Quantity = quantity;
        }

        public Guid MovementId { get; private set; }
        public Guid FileId { get; private set; }
        public DateTime DateReceied { get; private set; }
        public decimal Quantity { get; private set; }
    }
}
