﻿namespace EA.Iws.Requests.ImportNotification
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.ImportNotification;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportNotificationPermissions.CanChangeNumberOfShipmentsOnImportNotification)]
    public class GetChangeNumberOfShipmentConfrimationData : IRequest<ConfirmNumberOfShipmentsChangeData>
    {
        public GetChangeNumberOfShipmentConfrimationData(Guid notificationId, int numberOfShipments)
        {
            NotificationId = notificationId;
            NumberOfShipments = numberOfShipments;
        }

        public Guid NotificationId { get; private set; }

        public int NumberOfShipments { get; private set; }
    }
}
