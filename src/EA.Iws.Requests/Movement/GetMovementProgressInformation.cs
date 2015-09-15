﻿namespace EA.Iws.Requests.Movement
{
    using System;
    using Core.Movement;
    using Prsd.Core.Mediator;

    public class GetMovementProgressInformation : IRequest<MovementProgressAndSummaryData>
    {
        public Guid MovementId { get; private set; }

        public GetMovementProgressInformation(Guid movementId)
        {
            MovementId = movementId;
        }
    }
}
