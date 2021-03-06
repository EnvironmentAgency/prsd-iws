﻿namespace EA.Iws.Requests.Movement
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.Shared;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportMovementPermissions.CanReadExportMovementsInternal)]
    public class GetMovementAuditByNotificationId : IRequest<ShipmentAuditData>
    {
        public Guid NotificationId { get; private set; }

        public int PageNumber { get; private set; }

        public int? ShipmentNumber { get; private set; }

        public GetMovementAuditByNotificationId(Guid notificationId, int pageNumber, int? shipmentNumber = null)
        {
            NotificationId = notificationId;
            PageNumber = pageNumber;
            ShipmentNumber = shipmentNumber;
        }
    }
}
