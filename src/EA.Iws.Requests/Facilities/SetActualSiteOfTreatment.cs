﻿namespace EA.Iws.Requests.Facilities
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;
    using Security;

    [NotificationReadOnlyAuthorize]
    [RequestAuthorization(ExportNotificationPermissions.CanEditExportNotification)]
    public class SetActualSiteOfTreatment : IRequest<Guid>
    {
        public SetActualSiteOfTreatment(Guid facilityId, Guid notificationId)
        {
            FacilityId = facilityId;
            NotificationId = notificationId;
        }

        public Guid FacilityId { get; private set; }

        public Guid NotificationId { get; private set; }
    }
}
