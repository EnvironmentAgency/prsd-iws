﻿namespace EA.Iws.RequestHandlers.Movement
{
    using System.Data.Entity;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain;
    using Prsd.Core.Mediator;
    using RequestHandlers.Notification;
    using Requests.Movement;

    internal class GenerateMovementDocumentHandler : IRequestHandler<GenerateMovementDocument, byte[]>
    {
        private readonly IwsContext context;
        private readonly IMovementDocumentGenerator documentGenerator;

        public GenerateMovementDocumentHandler(IwsContext context, IMovementDocumentGenerator documentGenerator)
        {
            this.context = context;
            this.documentGenerator = documentGenerator;
        }

        public async Task<byte[]> HandleAsync(GenerateMovementDocument message)
        {
            var movement = await context.Movements.SingleAsync(m => m.Id == message.Id);
            var notification = await context.GetNotificationApplication(movement.NotificationId);
            var shipmentInfo = await context.GetShipmentInfoAsync(movement.NotificationId);

            var document = documentGenerator.Generate(movement, notification, shipmentInfo);

            return document;
        }
    }
}
