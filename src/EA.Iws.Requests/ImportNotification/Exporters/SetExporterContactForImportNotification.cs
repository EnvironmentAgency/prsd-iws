﻿namespace EA.Iws.Requests.ImportNotification.Exporters
{
    using System;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.ImportNotification.Summary;
    using Prsd.Core.Mediator;

    [RequestAuthorization(ImportNotificationPermissions.CanEditImportContactDetails)]
    public class SetExporterContactForImportNotification : IRequest<Unit>
    {
        public SetExporterContactForImportNotification(Guid importNotificationId, Contact contact)
        {
            ImportNotificationId = importNotificationId;
            Contact = contact;
        }

        public Guid ImportNotificationId { get; private set; }

        public Contact Contact { get; private set; }
    }
}
