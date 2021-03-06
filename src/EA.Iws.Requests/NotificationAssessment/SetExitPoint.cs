﻿namespace EA.Iws.Requests.NotificationAssessment
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ExportNotificationPermissions.CanChangeEntryExitPoint)]
    public class SetExitPoint : IRequest<Unit>
    {
        public SetExitPoint(Guid notificationId, Guid exitPointId)
        {
            NotificationId = notificationId;
            ExitPointId = exitPointId;
        }

        public Guid NotificationId { get; private set; }

        public Guid ExitPointId { get; private set; }
    }
}