﻿namespace EA.Iws.Requests.TransitState
{
    using System;
    using Authorization;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [NotificationReadOnlyAuthorize]
    [RequestAuthorization(ExportNotificationPermissions.CanEditExportNotification)]
    public class RemoveTransitStateForNotification : IRequest<bool>
    {
        public Guid NotificationId { get; private set; }

        public Guid TransitStateId { get; private set; }

        public RemoveTransitStateForNotification(Guid notificationId, Guid transitStateId)
        {
            NotificationId = notificationId;
            TransitStateId = transitStateId;
        }
    }
}