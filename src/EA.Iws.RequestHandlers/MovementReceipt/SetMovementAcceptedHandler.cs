﻿namespace EA.Iws.RequestHandlers.MovementReceipt
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.Movement;
    using Prsd.Core.Mediator;
    using Requests.MovementReceipt;

    internal class SetMovementAcceptedHandler : IRequestHandler<SetMovementAccepted, Guid>
    {
        private readonly IMovementRepository movementRepository;
        private readonly IwsContext context;

        public SetMovementAcceptedHandler(IMovementRepository movementRepository, IwsContext context)
        {
            this.movementRepository = movementRepository;
            this.context = context;
        }

        public async Task<Guid> HandleAsync(SetMovementAccepted message)
        {
            var movement = await movementRepository.GetById(message.MovementId);

            movement.Receive(message.FileId, message.DateReceived, message.Quantity);

            await context.SaveChangesAsync();

            return message.MovementId;
        }
    }
}
