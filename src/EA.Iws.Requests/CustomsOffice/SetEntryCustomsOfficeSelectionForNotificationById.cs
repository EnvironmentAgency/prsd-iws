﻿namespace EA.Iws.Requests.CustomsOffice
{
    using System;
    using Authorization;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Prsd.Core.Mediator;

    [NotificationReadOnlyAuthorize]
    [RequestAuthorization(ExportNotificationPermissions.CanEditExportNotification)]
    public class SetEntryCustomsOfficeSelectionForNotificationById : IRequest<bool>
    {
        public Guid Id { get; private set; }
        public bool Selection { get; private set; }

        public SetEntryCustomsOfficeSelectionForNotificationById(Guid id, bool selection)
        {
            Id = id;
            Selection = selection;
        }
    }
}