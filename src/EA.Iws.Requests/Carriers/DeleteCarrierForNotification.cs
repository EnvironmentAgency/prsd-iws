﻿namespace EA.Iws.Requests.Carriers
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;
    using Security;

    [NotificationReadOnlyAuthorize]
    [RequestAuthorization(ExportNotificationPermissions.CanEditExportNotification)]
    public class DeleteCarrierForNotification : IRequest<bool>
    {
        public DeleteCarrierForNotification(Guid notificationId, Guid carrierId)
        {
            NotificationId = notificationId;
            CarrierId = carrierId;
        }

        public Guid CarrierId { get; private set; }

        public Guid NotificationId { get; private set; }
    }
}