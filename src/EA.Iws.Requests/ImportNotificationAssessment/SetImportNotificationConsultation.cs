﻿namespace EA.Iws.Requests.ImportNotificationAssessment
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportNotificationPermissions.CanEditImportNotificationAssessment)]
    public class SetImportNotificationConsultation : IRequest<Guid>
    {
        public SetImportNotificationConsultation(Guid notificationId, Guid localAreaId)
        {
            LocalAreaId = localAreaId;
            NotificationId = notificationId;
        }

        public Guid LocalAreaId { get; private set; }

        public Guid NotificationId { get; private set; }
    }
}
