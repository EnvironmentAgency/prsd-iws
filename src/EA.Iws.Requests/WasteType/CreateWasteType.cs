﻿namespace EA.Iws.Requests.WasteType
{
    using System;
    using System.Collections.Generic;
    using Core.Authorization;
    using Core.Authorization.Permissions;
    using Core.WasteType;
    using Prsd.Core.Mediator;
    using Security;

    [NotificationReadOnlyAuthorize]
    [RequestAuthorization(ExportNotificationPermissions.CanEditExportNotification)]
    public class CreateWasteType : IRequest<Guid>
    {
        public Guid NotificationId { get; set; }

        public ChemicalCompositionType ChemicalCompositionType { get; set; }

        public string ChemicalCompositionDescription { get; set; }

        public List<WoodInformationData> WasteCompositions { get; set; }

        public string WasteCompositionName { get; set; }
    }
}